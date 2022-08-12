// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace MeGUI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public delegate void UpdateSourceDetectionStatus(int numDone, int total); // catches the UpdateGUI events fired from the encoder
    public delegate void FinishedAnalysis(SourceInfo info, ExitType exit, string errorMessage);

    public enum ExitType
    {
        OK,
        ERROR,
        ABORT
    };

    public class SourceInfo
    {
        public FieldOrder fieldOrder;
        public SourceType sourceType;
        public int decimateM;
        public bool majorityFilm;
        public bool isAnime;
        public string analysisResult;

        public SourceInfo()
        {
            decimateM = 1;
        }
    }

    public class SourceDetectorInfo
    {
        public int numTC;
        public int numProg;
        public int numInt;
        public int numUseless;
        public int sectionCount;
        public int sectionCountBFF;
        public int sectionCountTFF;
        public int[] numPortions;
        public int[] sectionsWithMovingFrames;
        public List<int[]>[] portions;

        public SourceDetectorInfo()
        {
            numTC = numProg = numInt = numUseless = 0;
            sectionCount = sectionCountBFF = sectionCountTFF = 0;
            numPortions = new int[2];
            sectionsWithMovingFrames = new int[6];

            // interlaced portions
            portions = new List<int[]>[2];
            portions[0] = new List<int[]>();
            portions[1] = new List<int[]>();
        }
    }

    public enum SourceType
    {
        UNKNOWN,
        NOT_ENOUGH_SECTIONS,
        PROGRESSIVE,
        INTERLACED,
        FILM,
        DECIMATING,
        HYBRID_FILM_INTERLACED,
        HYBRID_PROGRESSIVE_INTERLACED,
        HYBRID_PROGRESSIVE_FILM
    };

    public enum FieldOrder
    {
        UNKNOWN, TFF, BFF, VARIABLE, 
    };

    public class SourceDetector
    {
        public SourceDetector(string avsScript, string d2vFile, bool bIsAnime, int iFrameCount, ThreadPriority priority,
            SourceDetectorSettings oSettings, UpdateSourceDetectionStatus updateMethod, FinishedAnalysis finishedMethod)
        {
            script = avsScript;
            d2vFileName = d2vFile;
            settings = oSettings;
            isAnime = bIsAnime;
            frameCount = iFrameCount;
            trimmedFilteredLine = "";
            type = SourceType.UNKNOWN;
            majorityFilm = false;
            error = false;
            continueWorking = true;
            isStopped = false;
            oSourceInfo = new SourceDetectorInfo();
            this.priority = priority;

            analyseUpdate += updateMethod;
            finishedAnalysis += finishedMethod;
        }

        #region variables
        private bool isAnime;
        private bool error, continueWorking, isStopped;
        private bool majorityFilm;
        private string errorMessage = "";
        private SourceDetectorSettings settings;
        private event UpdateSourceDetectionStatus analyseUpdate;
        private event FinishedAnalysis finishedAnalysis;
        private string script, d2vFileName, trimmedFilteredLine;
        private int frameCount;
        private SourceType type;
        private int decimateM = 1;
        private int tffCount = 0, bffCount = 0;
        private FieldOrder fieldOrder = FieldOrder.UNKNOWN;
        private string analysis;
        private List<DeinterlaceFilter> filters = new List<DeinterlaceFilter>();
        private ManualResetEvent _mre = new System.Threading.ManualResetEvent(true); // lock used to pause processing
        private SourceDetectorInfo oSourceInfo;
        private const int sectionLength = 5; // number of frames in a section. do not change!
        private Thread analyseThread;
        private ThreadPriority priority;
        #endregion

        #region processing

        #region helper methods
        private string FindPortions(List<int[]> portions, int selectEvery, int selectLength, int numPortions,
            int sectionCount, int inputFrames, string type, out string trimLine, out int frameCount)
        {
            frameCount = 0;
            trimLine = "";
            trimmedFilteredLine = "";
            string outputText = string.Format("There are {0} {1} portions.\r\n", numPortions, type);

            int lastEndFrame = -1;
            for (int i = 0; i < numPortions; i++)
            {
                int portionStart = portions[i][0];
                int portionEnd = portions[i][1];
                int startFrame = Math.Max(0, (portionStart) * selectEvery);
                if (portionEnd == 0)
                    portionEnd = sectionCount;
                int endFrame = Math.Min(inputFrames-1, (portionEnd + 1) * selectEvery);
                frameCount += endFrame - startFrame;
                trimLine += string.Format("trim({0},{1}) ++ ", startFrame, endFrame);
                outputText += string.Format("Portion number {0} goes from frame {1} to frame {2}.\r\n", i + 1, startFrame, endFrame);
                trimmedFilteredLine += string.Format("original.trim({0},{1}) ++ deintted.trim({2},{3}) ++ ",
                    lastEndFrame + 1, startFrame - 1, startFrame, endFrame);
                lastEndFrame = endFrame;
            }
            if (lastEndFrame < inputFrames - 1)
                trimmedFilteredLine += string.Format("original.trim({0},{1})", lastEndFrame + 1, inputFrames);
            trimLine = trimLine.TrimEnd(new char[] { ' ', '+' });
            return outputText;
        }

        private void Process(string scriptBlock, string strLogFile, int scriptType)
        {
            try
            {
                using (AvsFile af = AvsFile.ParseScript(scriptBlock, false))
                {
                    int i = 0;
                    int frameCount = (int)af.VideoInfo.FrameCount;
                    bool running = true;

                    int iNextFrameCheck = frameCount;
                    if (settings.AnalysePercent == 0)
                    {
                        if (settings.MinimumAnalyseSections > settings.MinimumUsefulSections)
                            iNextFrameCheck = settings.MinimumAnalyseSections * sectionLength;
                        else
                            iNextFrameCheck = settings.MinimumUsefulSections * sectionLength;
                    }

                    new Thread(new ThreadStart(delegate
                    {
                        while (running && continueWorking)
                        {
                            if (analyseUpdate != null)
                            {
                                if (i >= iNextFrameCheck)
                                    analyseUpdate(iNextFrameCheck - 1, iNextFrameCheck);
                                else
                                    analyseUpdate(i, iNextFrameCheck);
                            }
                            MeGUI.core.util.Util.Wait(1000);
                        }
                    })).Start();

                    if (!continueWorking)
                    {
                        isStopped = true;
                        return;
                    }

                    IntPtr zero = new IntPtr(0);
                    if (settings.AnalysePercent == 0)
                    {
                        for (i = 0; i < frameCount && continueWorking; i++)
                        {
                            if (i > iNextFrameCheck)
                            {
                                if (scriptType == 0)
                                {
                                    if (!GetSectionCounts(strLogFile))
                                    {
                                        // error detected
                                        running = false;
                                        break;
                                    }

                                    if (oSourceInfo.numInt + oSourceInfo.numProg + oSourceInfo.numTC > settings.MinimumUsefulSections
                                        || CheckDecimate(oSourceInfo.sectionsWithMovingFrames))
                                    {
                                        running = false;
                                        break;
                                    }

                                    // no sufficient information yet
                                    // try to estimate when the next check should happen
                                    iNextFrameCheck = (int)((2 - (decimal)(oSourceInfo.numInt + oSourceInfo.numProg + oSourceInfo.numTC) / settings.MinimumUsefulSections) * i);
                                    if (iNextFrameCheck - i < 100)
                                        iNextFrameCheck += 100;
                                }
                                else
                                {
                                    if (!GetSectionCountsFF(strLogFile))
                                    {
                                        // error detected
                                        running = false;
                                        break;
                                    }

                                    if (oSourceInfo.sectionCountBFF + oSourceInfo.sectionCountTFF > settings.MinimumUsefulSections)
                                    {
                                        running = false;
                                        break;
                                    }

                                    // no sufficient information yet
                                    // try to estimate when the next check should happen
                                    iNextFrameCheck = (int)((2 - (decimal)(oSourceInfo.sectionCountBFF + oSourceInfo.sectionCountTFF) / settings.MinimumUsefulSections) * i);
                                    if (iNextFrameCheck - i < 100)
                                        iNextFrameCheck += 100;
                                }
                            }

                            _mre.WaitOne();
                            af.Clip.ReadFrame(zero, 0, i);
                        }
                    }
                    else
                    {
                        for (i = 0; i < frameCount && continueWorking; i++)
                        {
                            _mre.WaitOne();
                            af.Clip.ReadFrame(zero, 0, i);
                        }
                    }

                    if (!continueWorking)
                    {
                        isStopped = true;
                        return;
                    }

                    if (running)
                    {
                        if (scriptType == 0)
                            GetSectionCounts(strLogFile);
                        else
                            GetSectionCountsFF(strLogFile);
                        running = false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = true;
                errorMessage = "Error opening analysis script:\r\n" + ex.Message;
                FinishProcessing();
            }
        }
        #endregion

        #region script generation and running
        private void RunScript(int scriptType, string trimLine)
        {
            int selectEvery = sectionLength;
            if (settings.AnalysePercent > 0)
            {
                int minAnalyseSections = settings.MinimumAnalyseSections;
                if (settings.MinimumAnalyseSections < settings.MinimumUsefulSections)
                    minAnalyseSections = settings.MinimumUsefulSections;
                if (scriptType == 1)
                {
                    // Field order script. For this, we separatefields, so we have twice as many frames anyway
                    // It saves time, and costs nothing to halve the minimum sections to analyse for this example
                    minAnalyseSections = minAnalyseSections / 2 + 1; // We add one to prevent getting 0;
                }

                selectEvery = (int)((100.0 * (double)sectionLength) / ((double)settings.AnalysePercent));

                // check if we have to modify the SelectRangeEvery parameter
                // = if we are below the minimal to be analysed sections
                if (((double)frameCount / (double)selectEvery) < (int)minAnalyseSections)
                {
                    if (frameCount >= minAnalyseSections * sectionLength)
                    {
                        // there are more frames available as necessary for the minimal alaysis
                        selectEvery = (int)((double)frameCount / (double)minAnalyseSections);
                    }
                    else
                    {
                        // if there aren't enough frames, analyse everything
                        selectEvery = sectionLength;
                    }
                }
            }

            string logFileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), (scriptType == 1) ? "ff_interlace-" + Guid.NewGuid().ToString("N") + ".log" : "interlace-" + Guid.NewGuid().ToString("N") + ".log");

            if (File.Exists(logFileName))
                File.Delete(logFileName);

            string resultScript = ScriptServer.GetDetectionScript(scriptType, script, trimLine, logFileName, selectEvery, sectionLength);

            MethodInvoker mi = delegate {
                try
                {
                    Process(resultScript, logFileName, scriptType);

                    if (!continueWorking)
                    {
                        isStopped = true;
                        return;
                    }

                    if (error)
                        return;

                    if (scriptType == 0)
                        Analyse(selectEvery, sectionLength, frameCount); // detection
                    else
                        AnalyseFF(); // field order
                }
                finally
                {
                    try
                    {
                        File.Delete(logFileName);
                    }
                    catch(Exception)
                    {
                    }
                }
                isStopped = true;
            };

            analyseThread = new Thread(new ThreadStart(mi));
            analyseThread.Priority = priority;
            analyseThread.Start();
        }

        #endregion

        #region analysis
        private bool CheckDecimate(int[] data)
        {
            int[] dataCopy = new int[6];
            Array.Copy(data, dataCopy, 6);
            Array.Sort(dataCopy);

            int numMovingFrames = -1;
            for (int i = 0; i < data.Length; i++)
            {
                if (dataCopy[5] == data[i])
                    numMovingFrames = i;
            }

            if (dataCopy[5] > (double)dataCopy[4] * settings.DecimationThreshold &&
                numMovingFrames != 5 && numMovingFrames != 0)
                // If there are 5 moving frames, then it needs no decimation
                // If there are 0 moving frames, then we have a problem.
            {
                decimateM = 5 - numMovingFrames;
                return true;
            }
            return false;
        }

        private bool GetSectionCountsFF(string filename)
        {
            oSourceInfo.sectionCountTFF = oSourceInfo.sectionCountBFF = 0;
            tffCount = bffCount = 0;

            CultureInfo ci = new CultureInfo("en-us");
            StreamReader instream;
            try
            {
                instream = new StreamReader(filename);
            }
            catch (Exception)
            {
                instream = null;
                error = true;
                errorMessage = "Opening the field order analysis file failed.";
                FinishProcessing();
                return false;
            }

            int countA = 0, countB = 0, countEqual = 0;
            int localCountA = 0, localCountB = 0;
            double sumA = 0, sumB = 0;
            double valueA, valueB;
            int count = 0;

            string line = instream.ReadLine();
            while (line != null)
            {
                if (count != 0 && line.IndexOf("-1.#IND00") == -1) //Scene change or unexptected value -> ignore
                {
                    string[] contents = line.Split(new char[] { '-' });
                    try
                    {
                        valueA = Double.Parse(contents[0], ci);
                        valueB = Double.Parse(contents[1], ci);
                    }
                    catch (Exception)
                    {
                        error = true;
                        errorMessage = "Unexpected value in file " + filename + "\r\nLine contents: " + line;
                        FinishProcessing();
                        return false;
                    }
                    if (valueA > valueB)
                    {
                        countA++;
                        localCountA++;
                    }
                    else if (valueB > valueA)
                    {
                        countB++;
                        localCountB++;
                    }
                    else
                        countEqual++;
                    sumA += valueA;
                    sumB += valueB;
                }
                count++;
                if (count == 10)
                {
                    count = 0;
                    // Truly interlaced sections should always make one of the counts be 5 and the other 0.
                    // Progressive sections will be randomly distributed between localCountA and localCountB,
                    // so this algorithm successfully ignores those sections.
                    // Film sections will always have two frames which show the actual field order, and the other
                    // frames will show an arbitrary field order. This algorithm (luckily) seems to work very well
                    // with film sections as well. Using this thresholding as opposed to just comparing countB to countA
                    // produces _much_ more heavily-sided results.
                    if (localCountA > localCountB && localCountB == 0)
                        oSourceInfo.sectionCountTFF++;
                    if (localCountB > localCountA && localCountA == 0)
                        oSourceInfo.sectionCountBFF++;
                    localCountA = 0;
                    localCountB = 0;
                }
                line = instream.ReadLine();
            }
            instream.Close();

            tffCount = countA;
            bffCount = countB;

            return true;
        }

        private void AnalyseFF()
        {
            if ((((double)oSourceInfo.sectionCountTFF + (double)oSourceInfo.sectionCountBFF) / 100.0 * (double)settings.HybridFOPercent) > oSourceInfo.sectionCountBFF)
            {
                analysis += "Source is declared tff by a margin of " + oSourceInfo.sectionCountTFF + " to " + oSourceInfo.sectionCountBFF + ".";
                fieldOrder = FieldOrder.TFF;
            }
            else if ((((double)oSourceInfo.sectionCountTFF + (double)oSourceInfo.sectionCountBFF) / 100.0 * (double)settings.HybridFOPercent) > oSourceInfo.sectionCountTFF)
            {
                analysis += "Source is declared bff by a margin of " + oSourceInfo.sectionCountBFF + " to " + oSourceInfo.sectionCountTFF + ".";
                fieldOrder = FieldOrder.BFF;
            }
            else
            {
                analysis += "Source is hybrid bff and tff at " + oSourceInfo.sectionCountBFF + " bff and " + oSourceInfo.sectionCountTFF + " tff.";
                fieldOrder = FieldOrder.VARIABLE;
            }

            FinishProcessing();
        }

        private bool GetSectionCounts(string logFileName)
        {
            #region variable declaration
            bool[,] data = new bool[5, 2];
            int count = 0;
            // Decimation data
            int totalCombed = 0;
            int[] portionLength = new int[2];
            int[] nextPortionIndex = new int[2];
            bool[] inPortion = new bool[2];
            int[] portionStatus = new int[2];
            #endregion

            StreamReader instream;
            try
            {
                instream = new StreamReader(logFileName);
            }
            catch (Exception ex)
            {
                error = true;
                errorMessage = "Cannot open analysis log file \"" + logFileName + "\". error: " + ex.Message;
                FinishProcessing();
                return false;
            }

            oSourceInfo = new SourceDetectorInfo();

            #region loop
            string line = instream.ReadLine();
            while (line != null)
            {
                if (line.Length > 11)
                {
                    error = true;
                    errorMessage = "Unexpected value in file " + logFileName + ": " + line;
                    break;
                }

                string[] contents = line.Split(new char[] { '-' });
                data[count, 0] = (contents[0].Equals("true"));
                data[count, 1] = (contents[1].Equals("true"));
                count++;

                #region 5-ly analysis
                if (count == 5)
                {
                    oSourceInfo.sectionCount++;
                    int numComb = 0;
                    int numMoving = 0;
                    int combA = -1, combB = -1;
                    for (int i = 0; i < 5; i++)
                    {
                        if (data[i, 0])
                        {
                            numComb++;
                            if (combA == -1)
                                combA = i;
                            else
                                combB = i;
                        }
                        if (data[i, 1])
                            numMoving++;
                    }
                    totalCombed += numComb;
                    oSourceInfo.sectionsWithMovingFrames[numMoving]++;
                    if (numMoving < 5)
                    {
                        oSourceInfo.numUseless++;
                        portionStatus[0] = 1;
                        portionStatus[1] = 1;
                    }
                    else if (numComb == 2 && ((combB - combA == 1) || (combB - combA == 4)))
                    {
                        oSourceInfo.numTC++;
                        portionStatus[0] = 0;
                        portionStatus[1] = 2;
                    }
                    else if (numComb > 0)
                    {
                        oSourceInfo.numInt++;
                        portionStatus[0] = 2;
                        portionStatus[1] = 0;
                    }
                    else
                    {
                        oSourceInfo.numProg++;
                        portionStatus[0] = 0;
                        portionStatus[1] = 0;
                    }
                    #region portions
                    // Manage film and interlaced portions
                    for (int i = 0; i < 2; i++)
                    {
                        if (portionStatus[i] == 0) // Stop any portions we are in.
                        {
                            if (inPortion[i])
                            {
                                ((int[])oSourceInfo.portions[i][nextPortionIndex[i]])[1] = oSourceInfo.sectionCount;
                                #region useless comments
                                /*                                if (portionLength[i] == 1) // This should help reduce random fluctuations, by removing length 1 portions
 * I've now changed my mind about random fluctuations. I believe they are good, because they occur when TIVTC is on the verge of making
 * a wrong decision. Instead of continuing with this decision, which would then regard this section of the film as progressive, leaving combing
 * this now has the effect of dramatically increasing the number of portions, forcing the whole thing to be deinterlaced, which is better,
 * as it leaves no residual combing.
 * 
 * Edit again: i've left this section commented out, but the other section which removes length 1 progressive sections, I've left in, as it is
 * safer to deinterlace progressive stuff than vice versa.
                                {
                                    portions[i].RemoveAt(nextPortionIndex[i]);
                                    nextPortionIndex[i]--;
                                    numPortions[i]--;
                                }
*/
                                #endregion
                                nextPortionIndex[i]++;
                                inPortion[i] = false;
                            }
                            portionLength[i] = 0;
                        }
                        else if (portionStatus[i] == 1) // Continue all portions, but don't start a new one.
                        {
                            portionLength[i]++;
                        }
                        else if (portionStatus[i] == 2) // Start a new portion, or continue an old one.
                        {
                            if (inPortion[i])
                                portionLength[i]++;
                            else
                            {
                                int startIndex = oSourceInfo.sectionCount - portionLength[i];
                                int lastEndIndex = -2;
                                if (nextPortionIndex[i] > 0)
                                    lastEndIndex = ((int[])oSourceInfo.portions[i][nextPortionIndex[i] - 1])[1];
                                if (startIndex - lastEndIndex > 1) // If the last portion ended more than 1 section ago. This culls trivial portions
                                {
                                    oSourceInfo.portions[i].Add(new int[2]);
                                    ((int[])oSourceInfo.portions[i][nextPortionIndex[i]])[0] = startIndex;
                                    portionLength[i]++;
                                    oSourceInfo.numPortions[i]++;
                                }
                                else
                                {
                                    nextPortionIndex[i]--;
                                }
                                inPortion[i] = true;
                            }
                        }
                    }
                    #endregion
                    count = 0;
                }
                #endregion
                line = instream.ReadLine();
            }
            #endregion

            instream.Close();

            return true;
        }

        private void Analyse(int selectEvery, int selectLength, int inputFrames)
        {
            bool stillWorking = false;
 
            #region final counting
            int[] array = new int[] { oSourceInfo.numInt, oSourceInfo.numProg, oSourceInfo.numTC };
            Array.Sort(array);

            analysis = string.Format("Progressive sections: {0}\r\nInterlaced sections: {1}\r\nPartially Static Sections: {2}\r\nFilm Sections: {3}\r\n", oSourceInfo.numProg, oSourceInfo.numInt, oSourceInfo.numUseless, oSourceInfo.numTC);

            if (oSourceInfo.numInt + oSourceInfo.numProg + oSourceInfo.numTC < settings.MinimumUsefulSections)
            {
                if (CheckDecimate(oSourceInfo.sectionsWithMovingFrames))
                {
                    analysis += "Source is declared as repetition-upconverted. Decimation is required\r\n";
                    type = SourceType.DECIMATING;
                    FinishProcessing();
                    return;
                }
                else
                {
                    analysis += "Source does not have enough data. This either comes from an internal error or an unexpected source type.\r\n";
                    type = SourceType.NOT_ENOUGH_SECTIONS;
                    FinishProcessing();
                    return;
                }
            }

            #region plain
            if (array[1] < (double)(array[0]+array[1]+array[2]) /100.0 * settings.HybridPercent)
            {
                if (array[2] == oSourceInfo.numProg)
                {
                    if (!CheckDecimate(oSourceInfo.sectionsWithMovingFrames))
                    {
                        analysis += "Source is declared progressive.\r\n";
                        type = SourceType.PROGRESSIVE;
                    }
                    else
                    {
                        analysis += "Source is declared as repetition-upconverted. Decimation is required\r\n";
                        type = SourceType.DECIMATING;
                    }
                    FinishProcessing();
                    return;
                }
                else if (array[2] == oSourceInfo.numInt)
                {
                    analysis += "Source is declared interlaced.\r\n";
                    type = SourceType.INTERLACED;
                    stillWorking = true;
                    RunScript(1, "#no trimming"); //field order script
                }
                else
                {
                    analysis += "Source is declared telecined.\r\n";
                    type = SourceType.FILM;
                    stillWorking = true;
                    RunScript(1, "#no trimming"); //field order script
                }
            }
            #endregion
            #region hybrid
            else
            {
                if (array[0] == oSourceInfo.numProg) // We have a hybrid film/ntsc. This is the most common
                {
                    analysis += "Source is declared hybrid film/ntsc. Majority is ";
                    if (array[2] == oSourceInfo.numTC)
                    {
                        analysis += "film.\r\n";
                        majorityFilm = true;
                    }
                    else
                    {
                        analysis += "ntsc (interlaced).\r\n";
                        majorityFilm = false;
                    }
                    type = SourceType.HYBRID_FILM_INTERLACED;
                    stillWorking = true;
                    RunScript(1, "#no trimming");

                }
                else if (array[0] == oSourceInfo.numInt)
                {
                    if (array[0] > (double)(array[0]+array[1]+array[2]) / 100.0 * settings.HybridPercent) // There is also a section of interlaced
                    {
                        analysis += "Source is declared hybrid film/ntsc. Majority is film.\r\n";
                        type = SourceType.HYBRID_FILM_INTERLACED;
                        majorityFilm = true;
                        stillWorking = true;
                        RunScript(1, "#no trimming");
                    }
                    else
                    {
                        analysis += "Source is declared hybrid film/progressive.\r\n";
                        majorityFilm = (array[2] == oSourceInfo.numTC);
                        type = SourceType.HYBRID_PROGRESSIVE_FILM;

                        // Although we don't actually end up using portions for this situation, 
                        // it is good to only analyse the sections which are actually film.
                        int frameCount = -1;
                        string trimLine = "#no trimming";
                        string textLines = "The number of portions is " + oSourceInfo.numPortions[1] + ".\r\n";
                        if (oSourceInfo.numPortions[1] <= settings.MaxPortions)
                        {
                            textLines = FindPortions(oSourceInfo.portions[1], selectEvery, selectLength,
                                oSourceInfo.numPortions[1], oSourceInfo.sectionCount, inputFrames, "telecined", out trimLine, out frameCount);
                        }
                        stillWorking = true;
                        RunScript(1, trimLine);
                    }
                }
                else if (array[0] == oSourceInfo.numTC)
                {
                    if (array[0] > (double)(array[0] + array[1] + array[2]) / 100.0 * settings.HybridPercent) // There is also a section of film
                    {
                        analysis += "Source is declared hybrid film/interlaced. Majority is interlaced.\r\n";
                        type = SourceType.HYBRID_FILM_INTERLACED;
                        majorityFilm = false;

                        stillWorking = true;
                        RunScript(1, "#no trimming");
                    }
                    else
                    {
                        analysis += "Source is declared hybrid progressive/interlaced. ";
                        
                        type = SourceType.HYBRID_PROGRESSIVE_INTERLACED;

                        int frameCount = -1;
                        string trimLine = "#no trimming";
                        string textLines = "The number of portions is " + oSourceInfo.numPortions[0] + ".\r\n";

                        if (settings.PortionsAllowed &&
                            oSourceInfo.numPortions[0] <= settings.MaxPortions &&
                            array[2] < ((double)array[1] * settings.PortionThreshold))
                        {
                            textLines = FindPortions(oSourceInfo.portions[0], selectEvery, selectLength,
                                oSourceInfo.numPortions[0], oSourceInfo.sectionCount, inputFrames, "interlaced", out trimLine, out frameCount);
                            analysis += textLines;
                        }
                        else
                        {
                            analysis += "This should be deinterlaced by a deinterlacer that tries to weave it before deinterlacing.\r\n";
                        }
                        stillWorking = true;
                        RunScript(1, trimLine); //field order script
                    }
                }
            }
            #endregion
            #endregion

            if (!stillWorking)
                FinishProcessing();
        }
        #endregion

        #region finalizing
        private void FinishProcessing()
        {
            _mre.Set();  // Make sure nothing is waiting for pause to stop

            isStopped = true;

            if (error)
            {
                finishedAnalysis(null, ExitType.ERROR, errorMessage);
                return;
            }

            if (!continueWorking)
            {
                finishedAnalysis(null, ExitType.ABORT, String.Empty);
                return;
            }

            if (fieldOrder == FieldOrder.VARIABLE &&
                d2vFileName.Length == 0) // We are stuck for field order information, lets just go for what we have most of
                fieldOrder = (bffCount > tffCount) ? FieldOrder.BFF : FieldOrder.TFF;

            SourceInfo info = new SourceInfo();
            info.sourceType = type;
            if (type == SourceType.DECIMATING)
                info.decimateM = decimateM;
            info.fieldOrder = fieldOrder;
            info.majorityFilm = majorityFilm;
            info.analysisResult = analysis;
            info.isAnime = isAnime;

            finishedAnalysis(info, ExitType.OK, String.Empty);
        }
        #endregion

        #endregion

        #region program interface
        public void Analyse()
        {
            RunScript(0, "#no trimming");
        }

        public void Stop()
        {
            continueWorking = false;
            while (!isStopped)
                MeGUI.core.util.Util.Wait(500);
            FinishProcessing();
        }

        public bool Pause()
        {
            return _mre.Reset();
        }

        public bool Resume()
        {
            return _mre.Set();
        }

        public void ChangePriority(WorkerPriorityType priority)
        {
            try
            {
                if (analyseThread != null && analyseThread.IsAlive)
                    analyseThread.Priority = OSInfo.GetThreadPriority(priority);
            }
            catch (Exception) 
            {
                // process could not be running anymore - ignore
            }
        }

        #endregion
    }
}
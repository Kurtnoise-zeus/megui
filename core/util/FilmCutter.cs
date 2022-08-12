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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MeGUI.core.util
{
    internal class OverlappingSectionException : Exception
    {
        public OverlappingSectionException() : base("Sections overlap") { }
    }

    public class CutSection : IComparable<CutSection>
    {
        public int startFrame;
        public int endFrame;

        #region IComparable<CutSection> Members

        public int CompareTo(CutSection other)
        {
            if (other == this) return 0;
            if (other.startFrame == this.startFrame)
                throw new OverlappingSectionException();

            if (other.startFrame < this.startFrame)
            {
                if (other.endFrame < this.startFrame)
                    return 1;
                throw new OverlappingSectionException();
            }

            return 0 - other.CompareTo(this);

        }

        #endregion
    }
    public enum TransitionStyle
    {
        [EnumTitle("No transition")]
        NO_TRANSITION,
        [EnumTitle("Fade")]
        FADE,
        [EnumTitle("Dissolve")]
        DISSOLVE
    }

    [XmlInclude(typeof(CutSection))]
    public class Cuts
    {
        public void AdaptToFramerate(double newFramerate)
        {
            double ratio = newFramerate / Framerate;
            foreach (CutSection c in cuts)
            {
                c.startFrame = (int) ((double)c.startFrame * ratio);
                c.endFrame = (int) ((double)c.endFrame * ratio);
            }
            Framerate = newFramerate;
        }

        public int MinLength
        {
            get
            {
                if (cuts.Count == 0)
                    throw new Exception("Must have at least one cut");
                return cuts[cuts.Count - 1].endFrame + 1;
            }
        }

        public Cuts() { }

        private List<CutSection> cuts = new List<CutSection>();
        public double Framerate = -1;
        public TransitionStyle Style = TransitionStyle.FADE;

        public List<CutSection> AllCuts
        {
            get { return cuts; }
            set { cuts = value; }
        }

        public Cuts(TransitionStyle style)
        {
            this.Style = style;
        }

        public bool addSection(CutSection cut)
        {
            List<CutSection> old = new List<CutSection>(cuts);
            cuts.Add(cut);
            try
            {
                try
                {
                    cuts.Sort();
                }
                catch (InvalidOperationException e)
                { throw e.InnerException; }
            }
            catch (OverlappingSectionException) { cuts = old; return false; }

            return true;
        }

        public void Clear()
        {
            cuts.Clear();
        }

        public ulong TotalFrames
        {
            get
            {
                ulong ans = 0;
                foreach (CutSection c in AllCuts)
                    ans += (ulong)(c.endFrame - c.startFrame) + 1;
                return ans;
            }
        }

        public Cuts clone()
        {
            Cuts copy = new Cuts(Style);
            copy.cuts = new List<CutSection>(cuts);
            copy.Framerate = this.Framerate;
            return copy;
        }

        public void remove(CutSection cutSection)
        {
            cuts.Remove(cutSection);
        }
    }

    public class FilmCutter
    {
        public static void WriteCutsToFile(string filename, Cuts cuts)
        {
            Debug.Assert(cuts.AllCuts.Count > 0);
            using (Stream s = File.Open(filename, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Cuts));
                serializer.Serialize(s, cuts);
            }
        }

        public static Cuts ReadCutsFromFile(string filename)
        {
            using (Stream s = File.OpenRead(filename))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Cuts));
                return (Cuts)deserializer.Deserialize(s);
            }
        }

        public static string GetCutsScript(Cuts cuts, bool isAudio)
        {
            using (StringWriter s = new StringWriter())
            {
                s.WriteLine();
                s.WriteLine("__film = last");

                if (isAudio) // We need to generate a fake video track to add
                {
                    s.WriteLine("__just_audio = __film");
                    s.WriteLine("__blank = BlankClip(length={0}, fps={1})", cuts.MinLength, Math.Round(cuts.Framerate, 3).ToString(new CultureInfo("en-us")));
                    s.WriteLine("__film = AudioDub(__blank, __film)");
                }

                int counter = 0;
                foreach (CutSection c in cuts.AllCuts)
                {
                    s.WriteLine("__t{0} = __film.trim({1}, {2})", counter, c.startFrame, c.endFrame);
                    counter++;
                }

                if (cuts.Style == TransitionStyle.NO_TRANSITION)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        s.Write("__t{0} ", i);
                        if (i < counter - 1) s.Write("++ ");
                    }
                    s.WriteLine();
                }
                else if (cuts.Style == TransitionStyle.FADE)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        bool first = (i == 0);
                        bool last = (i == (counter - 1));
                        s.Write(addFades("__t" + i, first, last, isAudio));
                        if (!last) s.Write(" ++ ");
                    }
                    s.WriteLine();
                }
                else if (cuts.Style == TransitionStyle.DISSOLVE && counter != 0)
                {
                    string scriptlet = "__t" + (counter - 1);
                    for (int i = counter - 2; i >= 0; i--)
                    {
                        scriptlet = string.Format("__t{0}.Dissolve({1}, 60)", i, scriptlet);
                    }
                    s.WriteLine(scriptlet);
                }

                if (isAudio) // We now need to remove the dummy audio track
                {
                    // It will try to take the video from __just_audio, but there isn't any, so it just takes 
                    // the audio stream from last
                    s.WriteLine("AudioDubEx(__just_audio, last)");
                }
                return s.ToString();
            }
        }

        public static void WriteCutsToScript(string script, Cuts cuts, bool isAudio)
        {
            using (TextWriter s = new StreamWriter(File.Open(script, FileMode.Append)))
            {
                s.WriteLine(GetCutsScript(cuts, isAudio));
            }
        }


        private static string addFades(string p, bool first, bool last, bool isAudio)
        {
            if (first && last) 
                return p;
            if (isAudio)
            {
                if (!first && !last)
                    return string.Format("FadeIO0(FadeIO0({0}, 10, fps=AudioRate(__just_audio)), 10, fps=AudioRate(__just_audio))", p);
                if (first)
                    return string.Format("FadeOut0(FadeOut0({0}, 10, fps=AudioRate(__just_audio)), 10, fps=AudioRate(__just_audio))", p);
                if (last)
                    return string.Format("FadeIn0(FadeIn0({0}, 10, fps=AudioRate(__just_audio)), 10, fps=AudioRate(__just_audio))", p);
            }
            else
            {
                if (!first && !last)
                    return string.Format("FadeIO0({0}, 10, fps=AudioRate(__film))", p);
                if (first)
                    return string.Format("FadeOut0({0}, 10, fps=AudioRate(__film))", p);
                if (last)
                    return string.Format("FadeIn0({0}, 10, fps=AudioRate(__film))", p);
            }
            Debug.Assert(false);
            return null;
        }
    }
}
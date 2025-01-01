// ****************************************************************************
// 
// Copyright (C) 2005-2025 Doom9 & al
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
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace MeGUI.core.util
{
    [TypeConverter(typeof(DarConverter))]
    public struct Dar
    {
        public static readonly Dar ITU16x9PAL = new Dar(640, 351); // 720/576 * 512/351
        public static readonly Dar ITU4x3PAL = new Dar(160, 117); // 720/576 * 128/117
        public static readonly Dar ITU16x9NTSC = new Dar(8640, 4739); // 720/480 * 5760/4739
        public static readonly Dar ITU4x3NTSC = new Dar(6480, 4739); // 720/480 * 4320/4739
        public static readonly Dar STATIC4x3 = new Dar(4, 3);
        public static readonly Dar STATIC16x9 = new Dar(16, 9);
        public static readonly Dar A1x1 = new Dar(1, 1);

        private ulong x, y;
        private decimal ar;

        public Dar(ulong x, ulong y)
        {
            ar = (decimal)x / (decimal)y;
            this.x = x;
            this.y = y;
            RatioUtils.reduce(ref this.x, ref this.y);
        }

        public Dar(decimal dar)
        {
            ar = dar;
            RatioUtils.approximate(ar, out x, out y);
        }

        public Dar(decimal? dar, ulong width, ulong height)
        {
            ar = -1;
            if (dar.HasValue)
                ar = dar.Value;
            else
                ar = (decimal)width / (decimal)height;
            this.x = width;
            this.y = height;
            RatioUtils.reduce(ref this.x, ref this.y);
        }

        public Dar(int x, int y, ulong width, ulong height)
        {
            ar = -1;
            if (x > 0 && y > 0)
            {
                ar = (decimal)x / (decimal)y;
                this.x = (ulong)x;
                this.y = (ulong)y;
            }
            else
            {
                ar = (decimal)width / (decimal)height;
                this.x = width;
                this.y = height;
            }
            RatioUtils.reduce(ref this.x, ref this.y);
        }

        public decimal AR
        {
            get { return ar; }
            set { ar = value; }
        }

        public ulong X
        {
            get { return x; }
            set { x = value; }
        }

        public ulong Y
        {
            get { return y; }
            set { y = value; }
        }

        public override string ToString()
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");

            StringBuilder sb = new StringBuilder();
            sb.Append(x + ":" + y + " (");
            sb.Append(ar.ToString("0.000)", culture));
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Dar)) 
                return false;

            return (X == ((Dar)obj).X && Y == ((Dar)obj).Y);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public Sar ToSar(int hres, int vres)
        {
            // sarX
            // ----   must be the amount the video needs to be stretched horizontally.
            // sarY
            //
            //    horizontalResolution
            // --------------------------  is the ratio of the pixels. This must be stretched to equal realAspectRatio
            //  scriptVerticalResolution
            //
            // To work out the stretching amount, we then divide realAspectRatio by the ratio of the pixels:
            // sarX      parX        horizontalResolution        realAspectRatio * scriptVerticalResolution
            // ---- =    ---- /   -------------------------- =  --------------------------------------------   
            // sarY      parY     scriptVerticalResolution               horizontalResolution
            //
            // rounding value is mandatory here because some encoders (x264, xvid...) clamp sarX & sarY
            decimal ratio = ar * (decimal)vres / (decimal)hres;
            return new Sar(ratio);
        }
    }

    class DarConverter : TypeConverter
    {

        #region SimpleTypeConverter<Named<Dar>> Members

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return new Dar(decimal.Parse((string)value));

            throw new Exception();
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value.ToString();

            throw new Exception();
        }

        #endregion
    }

    public struct Sar
    {
        public decimal ar;

        public Sar(ulong x, ulong y)
        {
            ar = (decimal)x / (decimal)y;
        }

        public Sar(decimal sar)
        {
            ar = sar;
        }

        public ulong X
        {
            get
            {
                ulong x, y;
                RatioUtils.approximate(ar, out x, out y);
                return x;
            }
        }

        public ulong Y
        {
            get
            {
                ulong x, y; RatioUtils.approximate(ar, out x, out y);
                return y;
            }
        }

        public Dar ToDar(int hres, int vres)
        {
            return new Dar(ar * (decimal)hres / (decimal)vres);
        }
    }

    struct RatioUtils
    {
        /// <summary>
        /// Puts x and y in simplest form, by dividing by all their factors.
        /// </summary>
        /// <param name="x">First number to reduce</param>
        /// <param name="y">Second number to reduce</param>
        public static void reduce(ref ulong x, ref ulong y)
        {
            ulong g = gcd(x, y);
            x /= g;
            y /= g;
        }

        private static ulong gcd(ulong x, ulong y)
        {
            while (y != 0)
            {
                ulong t = y;
                y = x % y;
                x = t;
            }
            return x;
        }

        public static void approximate(decimal val, out ulong x, out ulong y)
        {
            approximate(val, out x, out y, 1.0E-5);
        }

        public static void approximate(decimal val, out ulong x, out ulong y, double precision)
        {
            // Fraction.Test();
            Fraction f = Fraction.toFract((double)val, precision);

            x = f.num;
            y = f.denom;

            reduce(ref x, ref y);
            // [i_a] ^^^ initial tests with the new algo show this is 
            // rather unnecessary, but we'll keep it anyway, just in case.
        }
    }
    /// <summary>
    /// <para>Code according to info found here: http://mathforum.org/library/drmath/view/51886.html</para>
    /// 
    /// <para>
    /// Date: 06/29/98 at 13:12:44</para>
    /// <para>
    /// From: Doctor Peterson</para>
    /// <para>
    /// Subject: Re: Decimal To Fraction Conversion</para>
    /// 
    /// <para>
    /// The algorithm I am about to show you has an interesting history. I 
    /// recently had a discussion with a teacher in England who had a 
    /// challenging problem he had given his students, and wanted to know what 
    /// others would do to solve it. The problem was to find the fraction 
    /// whose decimal value he gave them, which is essentially identical to 
    /// your problem! I wasn't familiar with a standard way to do it, but 
    /// solved it by a vaguely remembered Diophantine method. Then, my 
    /// curiosity piqued, and I searched the Web for information on the 
    /// problem and didn't find it mentioned in terms of finding the fraction 
    /// for an actual decimal, but as a way to approximate an irrational by a 
    /// fraction, where the continued fraction method was used. </para>
    /// 
    /// <para>
    /// I wrote to the teacher, and he responded with a method a student of 
    /// his had come up with, which uses what amounts to a binary search 
    /// technique. I recognized that this produced the same sequence of 
    /// approximations that continued fractions gave, and was able to 
    /// determine that it is really equivalent, and that it is known to some 
    /// mathematicians (or at least math historians). </para>
    /// 
    /// <para>
    /// After your request made me realize that this other method would be 
    /// easier to program, I thought of an addition to make it more efficient, 
    /// which to my knowledge is entirely new. So we're either on the cutting 
    /// edge of computer technology or reinventing the wheel, I'm not sure 
    /// which!</para>
    /// 
    /// <para>
    /// Here's the method, with a partial explanation for how it works:</para>
    /// 
    /// <para>
    /// We want to approximate a value m (given as a decimal) between 0 and 1, 
    /// by a fraction Y/X. Think of fractions as vectors (denominator, 
    /// numerator), so that the slope of the vector is the value of the 
    /// fraction. We are then looking for a lattice vector (X, Y) whose slope 
    /// is as close as possible to m. This picture illustrates the goal, and 
    /// shows that, given two vectors A and B on opposite sides of the desired 
    /// slope, their sum A + B = C is a new vector whose slope is between the 
    /// two, allowing us to narrow our search:</para>
    /// 
    /// <code>
    /// num
    /// ^
    /// |
    /// +  +  +  +  +  +  +  +  +  +  +
    /// |
    /// +  +  +  +  +  +  +  +  +  +  +
    /// |                                  slope m=0.7
    /// +  +  +  +  +  +  +  +  +  +  +   /
    /// |                               /
    /// +  +  +  +  +  +  +  +  +  +  D &lt;--- solution
    /// |                           /
    /// +  +  +  +  +  +  +  +  + /+  +
    /// |                       /
    /// +  +  +  +  +  +  +  C/ +  +  +
    /// |                   /
    /// +  +  +  +  +  + /+  +  +  +  +
    /// |              /
    /// +  +  +  +  B/ +  +  +  +  +  +
    /// |          /
    /// +  +  + /A  +  +  +  +  +  +  +
    /// |     /
    /// +  +/ +  +  +  +  +  +  +  +  +
    /// | /
    /// +--+--+--+--+--+--+--+--+--+--+--&gt; denom
    /// </code>
    /// 
    /// <para>
    /// Here we start knowing the goal is between A = (3,2) and B = (4,3), and 
    /// formed a new vector C = A + B. We test the slope of C and find that 
    /// the desired slope m is between A and C, so we continue the search 
    /// between A and C. We add A and C to get a new vector D = A + 2*B, which 
    /// in this case is exactly right and gives us the answer.</para>
    /// 
    /// <para>
    /// Given the vectors A and B, with slope(A) &lt; m &lt; slope(B), 
    /// we can find consecutive integers M and N such that 
    /// slope(A + M*B) &lt; x &lt; slope(A + N*B) in this way:</para>
    /// 
    /// <para>
    /// If A = (b, a) and B = (d, c), with a/b &lt; m &lt; c/d, solve</para>
    /// 
    /// <code>
    ///     a + x*c
    ///     ------- = m
    ///     b + x*d
    /// </code>
    /// 
    /// <para>
    /// to give</para>
    /// 
    /// <code>
    ///         b*m - a
    ///     x = -------
    ///         c - d*m
    /// </code>
    /// 
    /// <para>
    /// If this is an integer (or close enough to an integer to consider it 
    /// so), then A + x*B is our answer. Otherwise, we round it down and up to 
    /// get integer multipliers M and N respectively, from which new lower and 
    /// upper bounds A' = A + M*B and B' = A + N*B can be obtained. Repeat the 
    /// process until the slopes of the two vectors are close enough for the 
    /// desired accuracy. The process can be started with vectors (0,1), with 
    /// slope 0, and (1,1), with slope 1. Surprisingly, this process produces 
    /// exactly what continued fractions produce, and therefore it will 
    /// terminate at the desired fraction (in lowest terms, as far as I can 
    /// tell) if there is one, or when it is correct within the accuracy of 
    /// the original data.</para>
    /// 
    /// <para>
    /// For example, for the slope 0.7 shown in the picture above, we get 
    /// these approximations:</para>
    /// 
    /// <para>
    /// Step 1: A = 0/1, B = 1/1 (a = 0, b = 1, c = 1, d = 1)</para>
    /// 
    /// <code>
    ///         1 * 0.7 - 0   0.7
    ///     x = ----------- = --- = 2.3333
    ///         1 - 1 * 0.7   0.3
    /// 
    ///     M = 2: lower bound A' = (0 + 2*1) / (1 + 2*1) = 2 / 3
    ///     N = 3: upper bound B' = (0 + 3*1) / (1 + 3*1) = 3 / 4
    /// </code>
    /// 
    /// <para>
    /// Step 2: A = 2/3, B = 3/4 (a = 2, b = 3, c = 3, d = 4)</para>
    /// 
    /// <code>
    ///         3 * 0.7 - 2   0.1
    ///     x = ----------- = --- = 0.5
    ///         3 - 4 * 0.7   0.2
    /// 
    ///     M = 0: lower bound A' = (2 + 0*3) / (3 + 0*4) = 2 / 3
    ///     N = 1: upper bound B' = (2 + 1*3) / (3 + 1*4) = 5 / 7
    /// </code>
    /// 
    /// <para>
    /// Step 3: A = 2/3, B = 5/7 (a = 2, b = 3, c = 5, d = 7)</para>
    /// 
    /// <code>
    ///         3 * 0.7 - 2   0.1
    ///     x = ----------- = --- = 1
    ///         5 - 7 * 0.7   0.1
    /// 
    ///     N = 1: exact value A' = B' = (2 + 1*5) / (3 + 1*7) = 7 / 10
    /// </code>
    /// 
    /// <para>
    /// which of course is obviously right.</para>
    /// 
    /// <para>
    /// In most cases you will never get an exact integer, because of rounding 
    /// errors, but can stop when one of the two fractions is equal to the 
    /// goal to the given accuracy.</para>
    /// 
    /// <para>
    /// [...]Just to keep you up to date, I tried out my newly invented algorithm 
    /// and realized it lacked one or two things. Specifically, to make it 
    /// work right, you have to alternate directions, first adding A + N*B and 
    /// then N*A + B. I tested my program for all fractions with up to three 
    /// digits in numerator and denominator, then started playing with the 
    /// problem that affects you, namely how to handle imprecision in the 
    /// input. I haven't yet worked out the best way to allow for error, but 
    /// here is my C++ function (a member function in a Fraction class 
    /// implemented as { short num; short denom; } ) in case you need to go to 
    /// this algorithm.
    /// </para>
    /// 
    /// <para>[Edit [i_a]: tested a few stop criteria and precision settings;
    /// found that you can easily allow the algorithm to use the full integer
    /// value span: worst case iteration count was 21 - for very large prime
    /// numbers in the denominator and a precision set at double.Epsilon.
    /// Part of the code was stripped, then reinvented as I was working on a 
    /// proof for this system. For one, the reason to 'flip' the A/B treatment
    /// (i.e. the 'i&1' odd/even branch) is this: the factor N, which will
    /// be applied to the vector addition A + N*B is (1) an integer number to
    /// ensure the resulting vector (i.e. fraction) is rational, and (2) is
    /// determined by calculating the difference in direction between A and B.
    /// When the target vector direction is very close to A, the difference
    /// in *direction* (sort of an 'angle') is tiny, resulting in a tiny N
    /// value. Because the value is rounded down, A will not change. B will,
    /// but the number of iterations necessary to arrive at the final result
    /// increase significantly when the 'odd/even' processing is not included.
    /// Basically, odd/even processing ensures that once every second iteration
    /// there will be a major change in direction for any target vector M.]
    /// </para>
    /// 
    /// <para>[Edit [i_a]: further testing finds the empirical maximum
    /// precision to be ~ 1.0E-13, IFF you use the new high/low precision
    /// checks (simpler, faster) in the code (old checks have been commented out).
    /// Higher precision values cause the code to produce very huge fractions
    /// which clearly show the effect of limited floating point accuracy.
    /// Nevetheless, this is an impressive result.
    /// 
    /// I also changed the loop: no more odd/even processing but now we're
    /// looking for the biggest effect (i.e. change in direction) during EVERY
    /// iteration: see the new x1:x2 comparison in the code below.
    /// This will lead to a further reduction in the maximum number of iterations
    /// but I haven't checked that number now. Should be less than 21,
    /// I hope. ;-) ]
    /// </para>
    /// </summary>
    public struct Fraction
    {
        public ulong num;
        public ulong denom;

        public Fraction(ulong n, ulong d)
        {
            num = n;
            denom = d;
        }

        public static Fraction toFract(decimal val)
        {
            return Fraction.toFract((double)val, 1.0E-13 /* 1.0E-28 = decimal.epsilon */ );
        }

        public static Fraction toFract(float val)
        {
            return Fraction.toFract((double)val, 1.0E-13 /* float.Epsilon */ );
        }

        public static Fraction toFract(double val)
        {
            return Fraction.toFract(val, 1.0E-13 /* double.Epsilon */ );
        }

        public static Fraction toFract(double val, double Precision)
        {
            // find nearest fraction
            ulong intPart = (ulong)val;
            val -= (double)intPart;

            Fraction low = new Fraction(0, 1);           // "A" = 0/1 (a/b)
            Fraction high = new Fraction(1, 1);          // "B" = 1/1 (c/d)

            for (; ; )
            {
                Debug.Assert(low.Val <= val);
                Debug.Assert(high.Val >= val);

                //         b*m - a
                //     x = -------
                //         c - d*m
                double testLow = low.denom * val - low.num;
                double testHigh = high.num - high.denom * val;
                // test for match:
                // 
                // m - a/b < precision
                //
                // ==>
                //
                // b * m - a < b * precision
                //
                // which is happening here: check both the current A and B fractions.
                //if (testHigh < high.denom * Precision)
                if (testHigh < Precision) // [i_a] speed improvement; this is even better for irrational 'val'
                {
                    break; // high is answer
                }
                //if (testLow < low.denom * Precision)
                if (testLow < Precision) // [i_a] speed improvement; this is even better for irrational 'val'
                {
                    // low is answer
                    high = low;
                    break;
                }

                double x1 = testHigh / testLow;
                double x2 = testLow / testHigh;

                // always choose the path where we find the largest change in direction:
                if (x1 > x2)
                {
                    //double x1 = testHigh / testLow;
                    // safety checks: are we going to be out of integer bounds?
                    if ((x1 + 1) * low.denom + high.denom >= (double)long.MaxValue)
                    {
                        break;
                    }

                    ulong n = (ulong)x1;    // lower bound for m
                    //int m = n + 1;    // upper bound for m

                    //     a + x*c
                    //     ------- = m
                    //     b + x*d
                    ulong h_num = n * low.num + high.num;
                    ulong h_denom = n * low.denom + high.denom;

                    //ulong l_num = m * low.num + high.num;
                    //ulong l_denom = m * low.denom + high.denom;
                    ulong l_num = h_num + low.num;
                    ulong l_denom = h_denom + low.denom;

                    low.num = l_num;
                    low.denom = l_denom;
                    high.num = h_num;
                    high.denom = h_denom;
                }
                else
                {
                    //double x2 = testLow / testHigh;
                    // safety checks: are we going to be out of integer bounds?
                    if (low.denom + (x2 + 1) * high.denom >= (double)ulong.MaxValue)
                    {
                        break;
                    }

                    ulong n = (ulong)x2;    // lower bound for m
                    //ulong m = n + 1;    // upper bound for m

                    //     a + x*c
                    //     ------- = m
                    //     b + x*d
                    ulong l_num = low.num + n * high.num;
                    ulong l_denom = low.denom + n * high.denom;

                    //ulong h_num = low.num + m * high.num;
                    //ulong h_denom = low.denom + m * high.denom;
                    ulong h_num = l_num + high.num;
                    ulong h_denom = l_denom + high.denom;

                    high.num = h_num;
                    high.denom = h_denom;
                    low.num = l_num;
                    low.denom = l_denom;
                }
                Debug.Assert(low.Val <= val);
                Debug.Assert(high.Val >= val);
            }

            high.num += high.denom * intPart;
            return high;
        }

        public static void Test()
        {
            Fraction ret;
            double vut;

            vut = 0.1;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 0.99999997;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = (0x40000000 - 1.0) / (0x40000000 + 1.0);
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 1.0 / 3.0;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 1.0 / (0x40000000 - 1.0);
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 320.0 / 240.0;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 6.0 / 7.0;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 320.0 / 241.0;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 720.0 / 577.0;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 2971.0 / 3511.0;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 3041.0 / 7639.0;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = 1.0 / Math.Sqrt(2);
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
            vut = Math.PI;
            ret = Fraction.toFract(vut);
            Debug.Assert(Math.Abs(vut - ret.Val) < 1E-9);
        }

        public double Val
        {
            get
            {
                return (double)num / denom;
            }
        }
    }
}
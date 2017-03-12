using System;
using System.Text;

namespace NITW_Dialogue_Tool
{
    static class CompareNumeric
    {
        public static int sortFunc(this string s, string other)
        {
            if (s != null && other != null &&
                (s = s.Replace(" ", string.Empty)).Length > 0 &&
                (other = other.Replace(" ", string.Empty)).Length > 0)
            {
                int sIndex = 0, otherIndex = 0;

                while (sIndex < s.Length)
                {
                    if (otherIndex >= other.Length)
                        return 1;

                    if (char.IsDigit(s[sIndex]))
                    {
                        if (!char.IsDigit(other[otherIndex]))
                            return -1;

                        // Compare the numbers
                        StringBuilder sBuilder = new StringBuilder(), otherBuilder = new StringBuilder();

                        while (sIndex < s.Length && char.IsDigit(s[sIndex]))
                        {
                            sBuilder.Append(s[sIndex++]);
                        }

                        while (otherIndex < other.Length && char.IsDigit(other[otherIndex]))
                        {
                            otherBuilder.Append(other[otherIndex++]);
                        }

                        long sValue = 0L, otherValue = 0L;

                        try
                        {
                            sValue = Convert.ToInt64(sBuilder.ToString());
                        }
                        catch (OverflowException) { sValue = Int64.MaxValue; }

                        try
                        {
                            otherValue = Convert.ToInt64(otherBuilder.ToString());
                        }
                        catch (OverflowException) { otherValue = Int64.MaxValue; }

                        if (sValue < otherValue)
                            return -1;
                        else if (sValue > otherValue)
                            return 1;
                    }
                    else if (char.IsDigit(other[otherIndex]))
                        return 1;
                    else
                    {
                        int difference = string.Compare(s[sIndex].ToString(), other[otherIndex].ToString(), StringComparison.InvariantCultureIgnoreCase);

                        if (difference > 0)
                            return 1;
                        else if (difference < 0)
                            return -1;

                        sIndex++;
                        otherIndex++;
                    }
                }

                if (otherIndex < other.Length)
                    return -1;
            }

            return 0;
        }

    }
}

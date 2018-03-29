using System;
using System.Collections.Generic;
using System.Text;

namespace CubeSolver
{
    public static class StringHelper
    {
        public static string ReverseString(this string str)
        {
            string rStr = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                rStr += str[i];
            }
            return rStr;
        }

        public static string PieceListToString(List<Piece> pieceList)
        {
            string str = "";
            for (int i = 0; i < pieceList.Count; i++)
            {
                str += pieceList[i].ToShortString();
                if (i != pieceList.Count - 1) str += "; ";
            }
            return str;
        }


        public static void ThrowIfNull(this string str)
        {
            if (str == null) throw new Exception();
        }

        public static void ThrowIfNullOrEmpty(this string str)
        {
            ThrowIfNullOrBadLength(str);
        }

        public static void ThrowIfNullOrBadLength(this string str, int length = 0)
        {
            ThrowIfNull(str);
            if (str.Length != length) throw new Exception();
        }

    }
}

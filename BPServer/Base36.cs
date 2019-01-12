using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiopticPowerPathDicomServer
{
    // Log.Debug("base36 test of 12345: " + SpecimenCoding.SpecimenIDFromBarcode(SpecimenCoding.BarcodeFromSpecimenID(12345)));

    public class SpecimenCoding
    {
        static public string BarcodeFromSpecimenID(long specimen_id)
        {
            return "1" + Base36.Encode(specimen_id);    //note: material=-type prefix for specimens is 1
        }

        static public long SpecimenIDFromBarcode(string barcode)
        {
            string ToDecode = barcode.Trim().ToUpper().Substring(1);    // always uppercase, first character is material-type
            return Base36.Decode(ToDecode);
        }
    }

    // https://www.stum.de/2008/10/20/base36-encoderdecoder-in-c/
    public class Base36
    {
        private const string CHARACTERS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static public long Decode(string value)
        {
            List<char> database = new List<char>(CHARACTERS);
            List<char> tmp = new List<char>(value.ToUpper().TrimStart(new char[] { '0' }).ToCharArray());
            tmp.Reverse();

            long number = 0;
            int index = 0;
            foreach (char character in tmp)
            {
                number += database.IndexOf(character) * (long)Math.Pow(36, index);
                index++;
            }

            return number;
        }

        static public string Encode(long number)
        {
            List<char> database = new List<char>(CHARACTERS);
            List<char> value = new List<char>();
            long tmp = number;

            while (tmp != 0)
            {
                value.Add(database[Convert.ToInt32(tmp % 36)]);
                tmp /= 36;
            }

            value.Reverse();
            return new string(value.ToArray());
        }
    }
}


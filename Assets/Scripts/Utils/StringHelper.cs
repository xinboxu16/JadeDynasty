using System.Collections;
using System.Collections.Generic;
using System.Text;

public class StringHelper {

	public static int CalculateStringByte(string str)
    {
        if(str.Equals(string.Empty))
        {
            return 0;
        }

        int strlen = 0;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, bytes);
        int count = bytes.Length;
        for (int i = 0; i < count; ++i)
        {
            if(bytes[i] != 0)
            {
                ++strlen;
            }
        }
        return strlen;
    }
}

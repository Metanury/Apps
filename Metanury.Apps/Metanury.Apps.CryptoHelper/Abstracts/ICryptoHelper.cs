using System;
using System.Collections.Generic;
using System.Text;

namespace Metanury.Apps.CryptoHelper.Abstracts
{
    public interface ICryptoHelper
    {
        string SaltAdd(string targetString);

        string SaltRemove(string targetString);
    }
}

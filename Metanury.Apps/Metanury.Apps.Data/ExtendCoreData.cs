using Metanury.Apps.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Metanury.Apps.Data
{
    public static class ExtendCoreData
    {
        public static void SetReturnValue(this SqlCommand cmd)
        {
            cmd.AddParameterOutput("@Code", SqlDbType.BigInt);
            cmd.AddParameterOutput("@Value", SqlDbType.VarChar, 100);
            cmd.AddParameterOutput("@Msg", SqlDbType.NVarChar, 100);
        }

        public static void SetReturnValue(this SqlCommand cmd, int ValueSize)
        {
            cmd.AddParameterOutput("@Code", SqlDbType.BigInt);
            cmd.AddParameterOutput("@Value", SqlDbType.VarChar, ValueSize);
            cmd.AddParameterOutput("@Msg", SqlDbType.NVarChar, 100);
        }

        public static ReturnValue GetReturnValue(this SqlCommand cmd)
        {
            var result = new ReturnValue();
            result.Code = Convert.ToInt64(cmd.GetValue("@Code"));
            result.Value = Convert.ToString(cmd.GetValue("@Value"));
            result.Message = Convert.ToString(cmd.GetValue("@Msg"));
            if (result.Code > 0) result.Check = true;
            return result;
        }

        public static ReturnValue ExecuteReturnValue(this SqlCommand cmd)
        {
            cmd.SetReturnValue();
            cmd.ExecuteNonQuery();
            return cmd.GetReturnValue();
        }

        public static ReturnValue ExecuteReturnValue(this SqlCommand cmd, int ValueSize)
        {
            cmd.SetReturnValue(ValueSize);
            cmd.ExecuteNonQuery();
            return cmd.GetReturnValue();
        }
    }
}

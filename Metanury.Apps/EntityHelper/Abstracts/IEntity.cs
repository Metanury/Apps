using System;
using System.Collections.Generic;
using System.Text;

namespace EntityHelper
{
    public interface IEntity
    {
        string TableName { get; set; }
        string TargetColumn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Core.Models
{
    public enum SearchMethod
    {
        File = 0,
        Content = 1,
        RegEx = 2
    }

    public enum SizeConditionEnum
    {
        BiggerThan = 0,
        SmallerThan = 1,
        Equal = 2
    }

    public enum UnitOfMeasure
    {
        Bit = 0,
        Byte = 1,
        KiloByte = 2,
        MegaByte = 3,
        GigaByte = 4,
        TeraByte = 5
    }

    public enum DateCondition
    {
        LaterThan = 0,
        EarlierThan = 1,
        InThatDay = 2
    }

    public enum TaskState
    {
        Pending = 0,
        InProcess = 1,
        Ready = 2
    }
}

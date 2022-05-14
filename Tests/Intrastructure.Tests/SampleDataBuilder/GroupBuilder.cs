using Domain.Entities;
using System.Collections.Generic;

namespace Intrastructure.Tests.SampleDataBuilder
{
    
    public class GroupBuilder
    {
        public static string GroupName => "Group";
        public static decimal Capacity => 1000;

        public static Group WithDefaultValues()
        {
            return new Group(GroupName, Capacity);
        }
    }

}

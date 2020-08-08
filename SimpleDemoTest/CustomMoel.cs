using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemoTest
{
    [RzhDbHelper.DbAttribute.DataTable("CUSTOM")]
    public class CustomMoel
    {
        [RzhDbHelper.DbAttribute.DataProperty("ID")]
        public int Id { get; set; }

        [RzhDbHelper.DbAttribute.DataProperty("NAME")]
        public string Name { get; set; }

        [RzhDbHelper.DbAttribute.DataProperty("GENGER")]
        public bool Gender { get; set; }

        [RzhDbHelper.DbAttribute.DataProperty("PHONE")]
        public string Phone { get; set; }
    }
}

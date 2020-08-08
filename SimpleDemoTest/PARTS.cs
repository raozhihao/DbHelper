using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemoTest
{
    [RzhDbHelper.DbAttribute.DataTable("Parts")]
    public class Parts
    {
        [RzhDbHelper.DbAttribute.DataProperty("PID")]
        public string Id { get; set; }

        [RzhDbHelper.DbAttribute.DataProperty("PTNAME")]
        public string Name { get; set; }

        [RzhDbHelper.DbAttribute.DataProperty("PNUMS")]
        public int Num { get; set; }

        [RzhDbHelper.DbAttribute.DataProperty("PREMAINGNUM")]
        public int MarginNum { get; set; }

        [RzhDbHelper.DbAttribute.DataProperty("UPDATETIME")]
        public DateTime UpdateTime { get; set; }
    }
}

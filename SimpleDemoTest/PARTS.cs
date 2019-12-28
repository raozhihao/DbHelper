using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemoTest
{
    [DbHelper.DbAttribute.DataTable("Parts")]
    public class Parts
    {
        [DbHelper.DbAttribute.DataProperty("PID"),DbHelper.DbAttribute.DataPrimaryKey()]
        public string Id { get; set; }

        [DbHelper.DbAttribute.DataProperty("PTNAME")]
        public string Name { get; set; }

        [DbHelper.DbAttribute.DataProperty("PNUMS")]
        public int Num { get; set; }

        [DbHelper.DbAttribute.DataProperty("PREMAINGNUM")]
        public int MarginNum { get; set; }

        [DbHelper.DbAttribute.DataProperty("UPDATETIME")]
        public DateTime UpdateTime { get; set; }
    }
}

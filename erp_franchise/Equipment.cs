using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erp_franchise
{
    public class Equipment
    {
        private static List<List<string>> _equipment_detail = new List<List<string>>();

        public static List<List<string>> equipment_detail_property
        {
            get { return _equipment_detail; }
            set { _equipment_detail = value; }
        }

        //public static List<List<string>> create_equipment()
        //{
        //    List<List<string>> equ_list = new List<List<string>>();
        //    return equ_list;
        //}







        private static List<List<string>> _as_detail = new List<List<string>>();

        public static List<List<string>> as_detail_property
        {
            get { return _as_detail; }
            set { _as_detail = value; }
        }

        //public static List<List<string>> create_as()
        //{
        //    List<List<string>> as_list = new List<List<string>>();
        //    return as_list;
        //}






        private static List<List<string>> _store_detail = new List<List<string>>();

        public static List<List<string>> store_detail_property
        {
            get { return _store_detail; }
            set { _store_detail = value; }
        }
    }
}

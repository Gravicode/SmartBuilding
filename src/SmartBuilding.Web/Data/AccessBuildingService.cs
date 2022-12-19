using Microsoft.EntityFrameworkCore;
using SmartBuilding.Models;
using SmartBuilding.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuilding.Web.Data
{
    public class AccessBuildingService : ICrud<AccessBuilding>
    {
        SmartBuildingDB db;

        public AccessBuildingService()
        {
            if (db == null) db = new();

        }
        public long GetAccessBuildingCount()
        {
            return db.AccessBuildings.Count();
        }
        public bool DeleteData(object Id)
        {
            var selData = (db.AccessBuildings.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.AccessBuildings.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<AccessBuilding> FindByKeyword(string Keyword)
        {
            var data = from x in db.AccessBuildings
                       where x.Nama.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<AccessBuilding> GetAllData()
        {
            return db.AccessBuildings.OrderBy(x => x.Nama).ToList();
        }
     



        public AccessBuilding GetDataById(object Id)
        {
            return db.AccessBuildings.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(AccessBuilding data)
        {
            try
            {
                db.AccessBuildings.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(AccessBuilding data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                /*
                if (sel != null)
                {
                    sel.Nama = data.Nama;
                    sel.Keterangan = data.Keterangan;
                    sel.Tanggal = data.Tanggal;
                    sel.DocumentUrl = data.DocumentUrl;
                    sel.StreamUrl = data.StreamUrl;
                    return true;

                }*/
                return true;
            }
            catch
            {

            }
            return false;
        }

        public long GetLastId()
        {
            return db.AccessBuildings.Max(x => x.Id);
        }
    }

}
/*











 */

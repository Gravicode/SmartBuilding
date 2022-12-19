using SmartBuilding.Models;
using Microsoft.EntityFrameworkCore;
using SmartBuilding.StreamGrain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuilding.StreamGrain.Data
{
    public class FaceService : ICrud<Face>
    {
        SmartBuildingDB db;

        public FaceService()
        {
            if (db == null) db = new SmartBuildingDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Faces.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Faces.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Face> FindByKeyword(string Keyword)
        {
            var data = from x in db.Faces
                       where x.Nama.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Face> GetAllData()
        {
            return db.Faces.ToList();
        }

        public Face GetDataById(object Id)
        {
            return db.Faces.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Face data)
        {
            try
            {
                db.Faces.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Face data)
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
            return db.Faces.Max(x => x.Id);
        }
    }

}
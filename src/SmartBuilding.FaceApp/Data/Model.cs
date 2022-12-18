using EllipticCurve.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartBuilding.FaceApp.Data
{
    public class OutputCls
    {
        public OutputCls()
        {
            this.IsSucceed = false;
            this.Message = "";
        }
        public object Data { get; set; }
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
    }
    public class WebCamOptions
    {
        public int Width { get; set; } = 320;
        public string VideoID { get; set; }
        public string CanvasID { get; set; }
        public string Filter { get; set; } = null;
    }

    #region model db
    public class FaceTemp
    {
        public string nama { get; set; }
        public string url { get; set; }
    }
    public class Model
    {
    }
    [Table("face")]
    public class Face
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string Nama { get; set; }
        public string FileUrl { get; set; }
        public string FileUrl2 { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    #endregion
}

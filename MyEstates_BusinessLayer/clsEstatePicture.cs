using MyEstates_BusinessLayer.Enums;
using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public class clsEstatePicture :SaveData
    {
        private int _PictureID;
        private string _PictureName;    
        private string _PictureURL;
        private int _EstateID;
            
        public int PictureID  { get { return _PictureID; }  set { _PictureID = value; } }
        public string PictureName { get { return _PictureName; }  set { _PictureName = value; } }
        public string PictureURL { get { return _PictureURL; }  set { _PictureURL = value; } }
        public int EstateID { get { return _EstateID; }  set { _EstateID = value; } }


        private enMode Mode ;
        private enDeletetype deletetype;


        public clsEstatePicture()
        {
          PictureID = -1;
          PictureName = string.Empty;
          PictureURL = string.Empty;
          EstateID = -1;
          Mode = enMode.add;  
        }


        private clsEstatePicture(int PictureID ,string PictureName, string PictureURL ,int EstateID)
        {
            this.PictureID = PictureID;
            this.PictureName = PictureName;
            this.PictureURL = PictureURL;
            this.EstateID = EstateID;
            Mode = enMode.update;
        }



        public static clsEstatePicture Find(int PictureID)
        {
            string PictureName = string.Empty;
            string PictureURL = string.Empty;
            int EstateID = -1;

            int id = -1;

            id = clsEstatePictureDataAccessLayer.FindEstatePicture(ref PictureID , ref PictureName, ref PictureURL , ref EstateID);

            if (id != -1) {
                return new clsEstatePicture(PictureID, PictureName, PictureURL, EstateID);
            }

            return null;
        }

        // get Pictures Of Estate With EstateId  = 

        public DataTable getEstatePictures(int EstateID)
        {
            return clsEstatePictureDataAccessLayer.getPicturesOfEstateWithEstateID(EstateID);
        }




        // add delete update

        private bool _AddNewEstatePicture()
        {
            int id = -1;

            id = clsEstatePictureDataAccessLayer.AddNewEstatePictureToDB(this.PictureName, this.PictureURL, this.EstateID);

            if (id != -1) { return true; }

            return false;

        }

        private bool _UpdateEstatePicture()
        {
            return clsEstatePictureDataAccessLayer.UpdateEstatePictureInDB(this.PictureID,this.PictureName,this.PictureURL,this.EstateID);  
        }


        public bool DeleteEstatePicture(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return false;

            deletetype = ids.Count > 1 ? enDeletetype.deleteSingle : enDeletetype.deleteGroup;

            return _DeleteEstatePicture(ids, deletetype);

        }
        private bool _DeleteEstatePicture(List<int> ids, enDeletetype deleteGroup)
        {
            switch (deletetype)
            {

                case enDeletetype.deleteSingle:
                    return clsEstatePictureDataAccessLayer.DeleteSingleEstatePicture(ids[0]);
                case enDeletetype.deleteGroup:
                    return clsEstatePictureDataAccessLayer.DeleteMultiEstatePictures(ids);
            }

            return false;
        }


        public bool IfExsist()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.add:
                    if (_AddNewEstatePicture())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update: return _UpdateEstatePicture();
            }


            return false;
        }
    }
}

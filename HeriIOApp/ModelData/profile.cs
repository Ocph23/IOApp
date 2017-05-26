using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeriIOApp.ModelData
{
    [TableName("profile")]
    public class profile : BaseNotifyProperty
    {
        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChange("Id");
            }
        }

        [DbColumn("UserId")]
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChange("IdClient");
            }
        }

        [DbColumn("UserType")]
        public UserType UserType
        {
            get { return _usertype; }
            set
            {
                _usertype = value;
                OnPropertyChange("UserType");
            }
        }

        [DbColumn("Selogan")]
        public string Selogan
        {
            get { return _selogan; }
            set
            {
                _selogan = value;
                OnPropertyChange("Selogan");
            }
        }

        [DbColumn("UserPhoto")]
        public byte[] UserPhoto
        {
            get { return _userphoto; }
            set
            {
                _userphoto = value;
                OnPropertyChange("UserPhoto");
            }
        }

        [DbColumn("PageImage")]
        public byte[] PageImage
        {
            get { return _pageimage; }
            set
            {
                _pageimage = value;
                OnPropertyChange("PageImage");
            }
        }

        [DbColumn("Description")]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChange("Description");
            }
        }

        private int _id;
        private string _userId;
        private UserType _usertype;
        private string _selogan;
        private byte[] _userphoto;
        private byte[] _pageimage;
        private string _description;
    }

}

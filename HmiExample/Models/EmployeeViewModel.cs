using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace HmiExample.Models
{
    /// <summary>
    /// EmployeeViewModel is a selectable view model
    /// </summary>
    public class EmployeeViewModel : SelectableViewModel
    {
        private Guid _id;
        private string _displayName;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _code;
        private string _email;
        private string _photo;
        private byte[] _photoContent;
        private string _phoneNumber;

        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName == value) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Photo
        {
            get { return _photo; }
            set
            {
                if (_photo == value) return;
                _photo = value;
                OnPropertyChanged();
            }
        }

        public byte[] PhotoContent
        {
            get { return _photoContent; }
            set
            {
                if (_photoContent == value) return;
                _photoContent = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value) return;
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                if (_middleName == value) return;
                _middleName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber == value) return;
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage PhotoImage
        {
            get
            {
                if (PhotoContent != null && PhotoContent.Length > 0)
                {
                    //Initialize a memory stream from bytes
                    MemoryStream ms = new MemoryStream(PhotoContent);

                    // Create a BitmapSource
                    BitmapImage imgsrc = new BitmapImage();
                    imgsrc.BeginInit();
                    imgsrc.CacheOption = BitmapCacheOption.OnLoad;
                    imgsrc.StreamSource = ms;
                    imgsrc.EndInit();

                    //Close a memory stream
                    ms.Close();

                    return imgsrc;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Photo) && File.Exists(Photo.Trim()))
                    {
                        var filePath = Photo.Trim();

                        //Initialize a file stream to read the image file
                        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                        // Create a BitmapSource
                        BitmapImage imgsrc = new BitmapImage();
                        imgsrc.BeginInit();
                        imgsrc.CacheOption = BitmapCacheOption.OnLoad;
                        imgsrc.StreamSource = fs;
                        imgsrc.EndInit();

                        //Close a file stream
                        fs.Close();

                        return imgsrc;
                    }
                }
                return null;
            }
        }

        #region Constructors
        public EmployeeViewModel()
        {

        }

        public EmployeeViewModel(Employee employee)
        {
            _id = employee.Id;
            _displayName = employee.DisplayName;
            _firstName = employee.FirstName;
            _middleName = employee.MiddleName;
            _lastName = employee.LastName;
            _code = employee.Code;
            _email = employee.Email;
            _photo = employee.Photo;
            _photoContent = employee.PhotoContent;
            _phoneNumber = employee.PhoneNumber;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImportFromXD.CodeBehind;
using System.Threading;

namespace ImportFromXD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Thay Mac:
        //TH1: Có địa chỉ Mac đơn ở New thì luôn sử dụng địa chỉ này trong mọi trường hợp
        //TH2: Không có địa chỉ Mac đơn ở New: Sử dụng List Mac ở FromFile
        //TH3: Cả 2 ô TextBox (New và FromFile) đều không có giá trị thì không cài đặt giá trị Mac nữa, sử dụng Mac của máy (chức năng này để sau)

        //Khi click vào Change thì màu sắc của ô vuông đổi sang màu tím
        //Nếu đổi thành công thì chuyển sang màu xanh, thêm dấu tick ở giữa, click lần nữa thì chuyển lại về ban đầu
        //Nếu đổi không thành công thì chuyển sang màu đỏ và có thêm dấu nhân ở giữa, click lần nữa chuyển về ban đầu

        //Biến toàn cục lưu trạng thái của Rectangle_Change: mục đích để lựa chọn trạng thái tiếp theo sau khi click vào nó
        public Rectangle_Change_State recState = Rectangle_Change_State.Default;

        public MainWindow()
        {
            InitializeComponent();

            RefreshCurrentMac(true); //Load Mac hiện tại
            LoadFromFileBefore(); //Lấy đường dẫn file list Mac đã dùng trước đó
        }

        #region Giao diện
        #region Tạo chức năng chuyển tab: Tab Direct và Tab FromFile
        private void SetGridDirectMacVisibility(bool visible)
        {
            Grid gridDirectMac = (Grid)this.FindName("gridDirectMac");
            Grid gridFromFileMac = (Grid)this.FindName("gridFromFileMac");

            if (visible)
            {
                gridDirectMac.Visibility = Visibility.Visible;
                gridFromFileMac.Visibility = Visibility.Hidden;
            }
            else
            {
                gridDirectMac.Visibility = Visibility.Hidden;
                gridFromFileMac.Visibility = Visibility.Visible;
            }
        }

        private void Rectangle_TabLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetGridDirectMacVisibility(true);
        }

        private void Rectangle_TabRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetGridDirectMacVisibility(false);
        }
        #endregion
        
        /// <summary>
        /// Ẩn hiện các element mặc định
        /// </summary>
        /// <param name="show"></param>
        private void Show_gridChange_DefaultContent(bool show)
        {
            if (!show)
                gridChange.Children[1].Visibility = gridChange.Children[2].Visibility = Visibility.Hidden; //Ẩn 2 label ban đầu trên Rectangle
            else
                gridChange.Children[1].Visibility = gridChange.Children[2].Visibility = Visibility.Visible; //Hiện 2 label ban đầu trên Rectangle
        }

        /// <summary>
        /// Xoá các element không phải mặc định (xóa các element được thêm vào khi chuyển sang state khác)
        /// </summary>
        private void Remove_gridChange_NonDefaultContent()
        {
            int numOf_gridChange_Children = gridChange.Children.Count;
            gridChange.Children.RemoveRange(3, numOf_gridChange_Children - 3);
        }

        #region Thấy đổi trạng thái của rectangle Change
        private void Change_gridChange_ToChangingState()
        {
            recState = Rectangle_Change_State.Changing; //Đổi state của Rectangle_Change thành Changing

            Show_gridChange_DefaultContent(false);
            Remove_gridChange_NonDefaultContent();

            Rectangle rectangle = (Rectangle)gridChange.Children[0];
            rectangle.Fill = new SolidColorBrush(Colors.Purple);

            Label lblChangingIcon = new Label();
            lblChangingIcon.Width = lblChangingIcon.Height = 20;
            lblChangingIcon.Content = "";
            lblChangingIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            lblChangingIcon.FontSize = 20;
            lblChangingIcon.Foreground = new SolidColorBrush(Colors.White);
            lblChangingIcon.HorizontalAlignment = HorizontalAlignment.Left;
            lblChangingIcon.VerticalAlignment = VerticalAlignment.Top;
            double left = (gridChange.ActualWidth - lblChangingIcon.Width) / 2;
            double top = (gridChange.ActualHeight - lblChangingIcon.Height) / 2;
            lblChangingIcon.Margin = new Thickness(left, top, 0, 0);
            lblChangingIcon.Padding = new Thickness(0);

            gridChange.Children.Add(lblChangingIcon);
        }

        private void Change_gridChange_ToSuccessState()
        {
            recState = Rectangle_Change_State.Success; //Đổi trạng thái sang Success

            Show_gridChange_DefaultContent(false);
            Remove_gridChange_NonDefaultContent();

            Rectangle rectangle = (Rectangle)gridChange.Children[0];
            rectangle.Fill = new SolidColorBrush(Colors.Green);

            Label lblChangingIcon = new Label();
            lblChangingIcon.Width = lblChangingIcon.Height = 20;
            lblChangingIcon.Content = "";
            lblChangingIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            lblChangingIcon.FontSize = 20;
            lblChangingIcon.Foreground = new SolidColorBrush(Colors.White);
            lblChangingIcon.HorizontalAlignment = HorizontalAlignment.Left;
            lblChangingIcon.VerticalAlignment = VerticalAlignment.Top;
            double left = (gridChange.ActualWidth - lblChangingIcon.Width) / 2;
            double top = (gridChange.ActualHeight - lblChangingIcon.Height) / 2;
            lblChangingIcon.Margin = new Thickness(left, top, 0, 0);
            lblChangingIcon.Padding = new Thickness(0);

            gridChange.Children.Add(lblChangingIcon);
        }

        private void Change_gridChange_ToFailState()
        {
            recState = Rectangle_Change_State.Fail; //Đổi trạng thái sang Fail

            Show_gridChange_DefaultContent(false);
            Remove_gridChange_NonDefaultContent();

            Rectangle rectangle = (Rectangle)gridChange.Children[0];
            rectangle.Fill = new SolidColorBrush(Colors.Red);

            Label lblChangingIcon = new Label();
            lblChangingIcon.Width = lblChangingIcon.Height = 20;
            lblChangingIcon.Content = "";
            lblChangingIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            lblChangingIcon.FontSize = 20;
            lblChangingIcon.Foreground = new SolidColorBrush(Colors.White);
            lblChangingIcon.HorizontalAlignment = HorizontalAlignment.Left;
            lblChangingIcon.VerticalAlignment = VerticalAlignment.Top;
            double left = (gridChange.ActualWidth - lblChangingIcon.Width) / 2;
            double top = (gridChange.ActualHeight - lblChangingIcon.Height) / 2;
            lblChangingIcon.Margin = new Thickness(left, top, 0, 0);
            lblChangingIcon.Padding = new Thickness(0);

            gridChange.Children.Add(lblChangingIcon);
        }

        private void Change_gridChange_ToDefaultState()
        {
            recState = Rectangle_Change_State.Default;

            Show_gridChange_DefaultContent(true);
            Remove_gridChange_NonDefaultContent();

            Rectangle rectangle = (Rectangle)gridChange.Children[0];
            rectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0050C5")); //Chuyển Rectangle Change về trạng thái mặc định
        }
        #endregion

        #endregion


        #region Chức năng

        /// <summary>
        /// Check xem được sử dụng địa chỉ Mac từ đâu
        /// </summary>
        /// <returns></returns>
        public InputMacType ChooseWhichMacInputType()
        {
            InputMacType type;
            if (!string.IsNullOrEmpty(txtNewMac.Text))
                type = InputMacType.Direct;
            else
            {
                if (!string.IsNullOrEmpty(txtFile.Text))
                    type = InputMacType.FromFile;
                else
                {
                    type = InputMacType.None;
                }
            }

            return type;
        }

        /// <summary>
        /// Lấy chuỗi Mac mới
        /// </summary>
        /// <returns>Chuỗi Mac trả về có thể đã định dạng hay chưa định dạng đều được, có thể trả về null nếu muốn khôi phục Mac mặc định</returns>
        public string GetNewMacToReplace()
        {
            string resultMac = null; //Chuỗi Mac trả về có thể đã định dạng hay chưa định dạng đều được
            InputMacType inputType = ChooseWhichMacInputType();
            switch (inputType)
            {
                case InputMacType.Direct:
                    resultMac = txtNewMac.Text; //Mac ở bất kỳ dạng nào 
                    break;
                case InputMacType.FromFile:
                    string path = txtFile.Text;
                    resultMac = ReadListFile.Instance.GetNextMacFromFile(path); //Mac theo định dạng chung của list lưu trong file
                    break;
                case InputMacType.None:
                    resultMac = null; //Không có địa chỉ Mac mới
                    break;
            }
            return resultMac;
        }

        /// <summary>
        /// Cài đặt Mac mới cho máy, hoặc khôi phục về mặc định
        /// </summary>
        /// <returns>True: Nếu thay đổi thành công, False: Nếu không thể thay đổi Mac được</returns>
        public bool SetNewMac()
        {
            string newMac = GetNewMacToReplace(); //Mac này không cần định dạng lại vì phương thức SetMac đã định dạng ngay trước khi thay rồi
            try
            {
                if (newMac != null)
                {
                    MainWork.Instance.SetMac(newMac);
                    //Trong phương thức SetMac(newMac) có chuyển đổi newMac sang dạng không có dấu phân cách,
                    //do đó các trường hợp không thỏa mãn sẽ được trả về null, đồng thời, trước khi thay đổi có 
                    //kiểm trả newMac != null mới thay đổi, do đó nếu newMac không thỏa mãn thì sẽ không đổi
                }
                else
                {
                    MainWork.Instance.RestoreDefaultMac();
                }

                MainWork.Instance.DisableAndEnableNetworkAdapter(); //Disable rồi Enable EthernetAdapter

                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message); //Cái này cho mục đích thử lỗi
                return false;
            }
        }

        /// <summary>
        /// Thực hiện thay địa chỉ Mac, thay đổi trạng thái của Rectangle_Change dựa trên kết quả từ SetNewMac()
        /// </summary>
        public void ChangeMacAndShowResult()
        {
            bool result = SetNewMac();
            if(result == true) //Nếu thay đổi thành công
            {
                Change_gridChange_ToSuccessState();
            }
            else
            {
                Change_gridChange_ToFailState();
            }
            RefreshCurrentMac(false);
        }

        /// <summary>
        /// Đưa lastPosition trở về 0 khi sử dụng file list Mac mới, thay đổi LastPosition trong lúc Runtime - ReadListFile
        /// </summary>
        public void ResetPositionWhenNewFileLoaded()
        {
            ReadListFile.Instance.LastPosition = 0;
        }


        #region Tải lại các thông tin cần thiết khi Window mở lên

        /// <summary>
        /// Lấy Mac hiện tại đang sử dụng đưa vào lblCurrentValue
        /// </summary>
        /// <param name="canInDisableCase">False nếu Refresh Mac ngay sau khi đổi Mac băng phương thức DisableAndEnableNetworkAdapter()</param>
        public void RefreshCurrentMac(bool canInDisableCase)
        {
            if (!canInDisableCase) //TH này xảy ra ngay sau khi Disable rồi Enable khi đổi Mac

            {
                while (!MainWork.Instance.IsEthernetAdapterEnabled())
                {
                    Thread.Sleep(500);
                }
            }

            //Đến đây thì trạng thái của EthernetAdapter đã ổn định, hoặc là Disabled: trả về null khi gán lblCurrentValue.Content,
            //hoặc giá trị đang dùng của Mac nếu Enabled
            string macFromRegistry = MainWork.Instance.GetMacViaNetworkInterface(); //Đây là dạng không có dấu phân cách
            lblCurrentValue.Content = MainWork.Instance.ConvertMac(macFromRegistry, true); //Đã được định dạng để hiển thị lên Label
        }

        /// <summary>
        /// Tải đường dẫn của file List Mac đã dùng ở lần trước
        /// </summary>
        public void LoadFromFileBefore()
        {
            txtFile.Text = AppSettings.Instance.PathListFile;
        }

        /// <summary>
        /// Lưu đường dẫn hiện tại vào config
        /// </summary>
        public void SaveCurrentFromFile()
        {
            AppSettings.Instance.PathListFile = txtFile.Text;
        }

        /// <summary>
        /// Lưu vị trí dòng Mac cuối cùng đã đọc, lấy từ ReadListFile
        /// </summary>
        public void SaveLastPosition()
        {
            AppSettings.Instance.LastPosition = ReadListFile.Instance.LastPosition;
        }
        #endregion

        #endregion


        private void lblRefreshMacButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RefreshCurrentMac(true);
        }

        private void gridChange_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (recState)
            {
                case Rectangle_Change_State.Default:
                    Change_gridChange_ToChangingState();
                    ChangeMacAndShowResult();
                    break;
                case Rectangle_Change_State.Changing:
                    //Click lúc đang ở trạng thái đang thay đổi thì không làm gì hết
                    break;
                case Rectangle_Change_State.Success:
                case Rectangle_Change_State.Fail:
                    Change_gridChange_ToDefaultState();
                    break;
            }
        }

        private void lblBrowseFileButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string fileName = dlg.FileName;
                txtFile.Text = fileName;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveCurrentFromFile(); //Lưu đường dẫn file list Mac vào config
            SaveLastPosition(); //Lưu dòng đã đọc lần trước
            //Không cần lưu EthernetName vì cái này chỉ đọc, người dùng sẽ tự thay đổi nếu cần thiết

            AppSettings.Instance.WriteConfig(); //Lưu lại config trước khi đóng file
        }

        private void txtFile_TextChanged(object sender, TextChangedEventArgs e)
        {   //Sử dụng sự kiện này để reset LastPosition vì: sự kiện chỉ xả ra khi text trong txtFile thay đổi, 
            //nếu có giá trị mới được gán vào nhưng giống hết giá trị cũ thì sự kiện vẫn không xảy ra

            if (this.IsLoaded) //Không cho sự kiện xảy ra ở lần load dữ liệu từ config
            {
                if (string.IsNullOrEmpty(txtFile.Text))
                {

                }
                else
                {
                    
                    ResetPositionWhenNewFileLoaded();
                }
            }

            if (!string.IsNullOrEmpty(txtFile.Text))
            {
                lblClear_txtFile.Visibility = Visibility.Visible; //Do trong txtFile hiện đã có text nên phải hiện label xóa
            }
        }

        private void lblClear_txtFile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Sử dụng để xóa giá trị trong TextBox txtFile
            txtFile.Clear();
            lblClear_txtFile.Visibility = Visibility.Hidden; //Ẩn label xóa khi không có text trong txtFile
        }
    }

    /// <summary>
    /// Enum dùng để lựa chọn Mac được lấy từ đâu
    /// </summary>
    public enum InputMacType
    {
        Direct,
        FromFile,
        None
    }

    /// <summary>
    /// Enum dùng để xác định Rectangle state
    /// </summary>
    public enum Rectangle_Change_State
    {
        Default,      //Đây là trạng thái mặc định của Rectangle_Change
        Changing,     //Trạng thái Đang thay đổi
        Success,      //Trạng thái sau khi thay đổi thành công
        Fail          //Trạng thái sau khi thay đổi thất bại
    }
}

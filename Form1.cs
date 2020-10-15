using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reminder
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon { get; set; }
        private SynchronizationContext synchronizationContext { get; set; }
        public Form1()
        {
            InitializeComponent();
            InitializeTray();
            InitializeReminder();
            synchronizationContext = WindowsFormsSynchronizationContext.Current;
        }

        private async void InitializeReminder()
        {
            await Reminder.Init();
            await Reminder.Instance.AddDrinkWaterJob(ShowForm);
        }

        private void InitializeTray()
        {
            var contextMenu = new ContextMenu();
            var menuItem = new MenuItem()
            {
                Text = "Desligar"
            };
            menuItem.Click += MenuItem_Click;

            contextMenu.MenuItems.Add(menuItem);

            notifyIcon = new NotifyIcon()
            {
                Icon = new System.Drawing.Icon(@"Images\drop.ico"),
                Visible = true,
                ContextMenu = contextMenu
            };
        }

        private void MenuItem_Click(object sender, System.EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }

        private void ShowForm()
        {
            synchronizationContext.Post((f) => {
                Visible = true;
            }, new { });
        }

        private void HideForm()
        {
            synchronizationContext.Post((f) => {
                Visible = false;
            }, new { });
        }

        private void btnMinimize_Click(object sender, System.EventArgs e)
        {
            HideForm();
        }

        private async void btnRemindMe_Click(object sender, System.EventArgs e)
        {
            await Reminder.Instance.AddQuickDrinkWaterJob(ShowForm);
            HideForm();
        }
    }
}

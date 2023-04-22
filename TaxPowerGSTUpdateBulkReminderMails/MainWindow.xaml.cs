using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MailKit.Net.Smtp;

namespace TaxPowerGSTUpdateBulkReminderMails
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnSendBulkMails_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                                
                DataTable dtRenewalPending = new DataTable();

                string query = string.Empty;

                query = "Select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SrNo,Clients.ID as ClientID,UYearID,PartnerID,Clients.CompanyName,Clients.CustomerID,CAST(DecryptByPassphrase ('MAGTPED$998#540@1',Clients.Password) as varchar(max)) as Password,Clients.ContactPerson,Clients.MobileNo1,Clients.MobileNo2,Clients.PhoneNo,Clients.EmailID1,Clients.EmailID2,Clients.RegistrationDate,Clients.BlockFlatNo+' '+Clients.BuildingName+' '+Clients.StreetRoad+' '+Clients.AreaLocality Address,Clients.City,ClientType,Products.ProductName,Licenses.ProductModule,ProductType,ReportingDate,ProductUsageAnalytics.ProductVersion,Partners.CompanyName as PartnerName from TaxPowerCRm.dbo.Clients LEFT JOIN TaxPowerCRM.dbo.Licenses ON Licenses.ClientID=Clients.ID LEFT JOIN TaxPowerCRM.dbo.ProductUsageAnalytics ON ProductUsageAnalytics.ClientID=Clients.ID LEFT JOIN TaxPowerCRM.dbo.Partners ON Clients.PartnerID=Partners.ID LEFT JOIN TaxPowerCRM.dbo.Products ON Products.ID=Licenses.ProductID Where Clients.ClientStatus=@ClientStatus and Licenses.LicenseStatus=@LicenseStatus and Licenses.UyearID=@UyearID Order By ReportingDate DESC";

                dtRenewalPending = await SQLDataAccess.SQLFillDataTableAsync(query, CM => { CM.Parameters.AddWithValue("ClientStatus", "Active"); CM.Parameters.AddWithValue("LicenseStatus", "Active"); CM.Parameters.AddWithValue("UyearID", 18); });

                pb1.Maximum = dtRenewalPending.Rows.Count;

                for (int i = 0; i <= 500; i++)
                {
                    int clientID = 0;
                    string? companyName = string.Empty;
                    string? customerID = string.Empty;
                    string? password = string.Empty;
                    string? city = string.Empty;
                    string? contactPerson = string.Empty;
                    string? clientType = string.Empty;
                    string? emailID = string.Empty;
                    string? productType = string.Empty;
                    string? productModule = string.Empty;

                    clientID = Convert.ToInt32(dtRenewalPending.Rows[i]["ClientID"]);
                    companyName = Convert.ToString(dtRenewalPending.Rows[i]["CompanyName"]);
                    customerID = Convert.ToString(dtRenewalPending.Rows[i]["CustomerID"]);
                    password = Convert.ToString(dtRenewalPending.Rows[i]["Password"]);
                    city = Convert.ToString(dtRenewalPending.Rows[i]["City"]);
                    contactPerson = Convert.ToString(dtRenewalPending.Rows[i]["ContactPerson"]);
                    clientType = Convert.ToString(dtRenewalPending.Rows[i]["ClientType"]);
                    emailID = Convert.ToString(dtRenewalPending.Rows[i]["EmailID1"]);
                    productType = Convert.ToString(dtRenewalPending.Rows[i]["ProductType"]);
                    productModule = Convert.ToString(dtRenewalPending.Rows[i]["ProductModule"]);

                    if (emailID == null || string.IsNullOrEmpty(emailID))
                    {
                        emailID = Convert.ToString(dtRenewalPending.Rows[i]["EmailID2"]);

                        if (emailID!=null && emailID.Equals("milindandassociates@gmail.com"))
                        {
                            continue;
                        }
                    }                    

                    try
                    {
                        await SendEmail(clientID, companyName, customerID, password, city, contactPerson, emailID, productType, productModule);

                        pb1.Value = i;
                        lblProgress.Text = i.ToString() + "/" + pb1.Maximum;
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                    
                }

                MessageBox.Show("Completed Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private async Task SendEmail(int clientID, string companyName, string customerID, string password, string city, string contactPerson, string emailID, string productType, string productModule)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    using (MimeMessage mimeMessage = new MimeMessage())
                    {
                        mimeMessage.From.Add(new MailboxAddress("Magnum Infosystem Pvt. Ltd.", "info@magnuminfosystem.com"));

                        //string clientEmailID = string.Empty;
                        //clientEmailID = Convert.ToString(SQLDataAccess.SQLExecuteScalar("Select EmailID1 from Clients Where ClientID=@ClientID", CM => { CM.Parameters.AddWithValue("ClientID", Convert.ToString(Session["ClientID"])); }));

                        mimeMessage.To.Add(new MailboxAddress("", emailID));

                        mimeMessage.Subject = "Reminder for updation of TaxPower GST Software for F.Y. 2023-2024";

                        if (string.IsNullOrWhiteSpace(contactPerson))
                        {
                            contactPerson = "Customer";
                        }

                        string encUserName = string.Empty;
                        string encPassword = string.Empty;

                        encUserName = HttpUtility.UrlEncode(EncryptDecrypt.Encrypt(customerID, "MAGTPED$998#540@1"));
                        encPassword = HttpUtility.UrlEncode(EncryptDecrypt.Encrypt(password, "MAGTPED$998#540@1"));

                        string mailheader = "To," + Environment.NewLine + companyName + Environment.NewLine + "Customer ID: " + customerID + Environment.NewLine + city + Environment.NewLine + "Date: " + DateTime.Now.ToShortDateString() + Environment.NewLine + Environment.NewLine + "Sub:- Reminder for updation of TaxPower GST Software for F.Y. 2023-2024" + Environment.NewLine + Environment.NewLine + "Dear " + contactPerson + Environment.NewLine + Environment.NewLine + "Greetings from Magnum Infosystem";
                        string mailBody = "We appreciated having the opportunity to serve you as a valued customer." + Environment.NewLine + "It is to remind you that Updation of TaxPower GST software for Financial Year 2023-2024 has been released." + Environment.NewLine + Environment.NewLine + "Product Details:" + Environment.NewLine + "Product Name: TaxPower GST" + Environment.NewLine + "Product Type: " + productType + Environment.NewLine + "Product Module: " + productModule + Environment.NewLine + Environment.NewLine + "You may make the payment via:" + Environment.NewLine + Environment.NewLine + "Payment Modes:" + Environment.NewLine + "     1) Online Payment through Net Banking/Debit Cards/Credit Cards/UPI " + Environment.NewLine + "     2) Cheque or Demand Draft in favour of Magnum Infosystem Pvt. Ltd." + Environment.NewLine + "     3) Online Transfer to Magnum Infosystem Bank Account through IMPS/NEFT/RTGS." + Environment.NewLine + Environment.NewLine + "Click on link to make Online Payment or Submit Offline Payment Details : https://www.magnuminfosystem.com/userlogin?username=" + encUserName + "&password=" + encPassword + Environment.NewLine + Environment.NewLine + "After making payment you can Renew your License by clicking on Help button then click on Activate Product then click on Update License." + Environment.NewLine + Environment.NewLine + "Note: Cash Payments are not acceptable" + Environment.NewLine + Environment.NewLine + "Pay to : Magnum Infosystem Pvt. Ltd." + Environment.NewLine + "Bank Details for Depositing Cheque or Online Payment:" + Environment.NewLine + "Bank Name: HDFC Bank" + Environment.NewLine + "A/c No: 05022000008113" + Environment.NewLine + "Branch Name: Central Avenue, Nagpur" + Environment.NewLine + "IFSC Code: HDFC0000502" + Environment.NewLine + Environment.NewLine + "Bank Name: ICICI Bank" + Environment.NewLine + "A/c No: 005905017359" + Environment.NewLine + "Branch Name: Civil Lines, Nagpur" + Environment.NewLine + "IFSC Code: ICIC0000059" + Environment.NewLine + Environment.NewLine + "For any queries related to Billing, please contact our customer care." + Environment.NewLine + Environment.NewLine + "Magnum Infosystem Pvt. Ltd." + Environment.NewLine + "Office No: 20FL, 3rd Floor, Manorama Tower," + Environment.NewLine + "Bharat Mata Chowk," + Environment.NewLine + "Jagnath Budhwari Nagpur-440002" + Environment.NewLine + "Ph: 9811881661, 0712-2724404" + Environment.NewLine + "Email: info@magnuminfosystem.com";
                        string mailFooter = "Regards" + Environment.NewLine + "Magnum Infosystem Pvt. Ltd." + Environment.NewLine + "Nagpur";

                        BodyBuilder builder = new BodyBuilder();

                        builder.TextBody = mailheader + Environment.NewLine + Environment.NewLine + mailBody + Environment.NewLine + Environment.NewLine + mailFooter;

                        mimeMessage.Body = builder.ToMessageBody();

                        await smtpClient.ConnectAsync("smtp.gmail.com", 465);
                        await smtpClient.AuthenticateAsync("info@magnuminfosystem.com", "qwbpfheofipqjzuz");
                        await smtpClient.SendAsync(mimeMessage);
                        await smtpClient.DisconnectAsync(true);

                        //SQLDataAccess.SQLExecuteNonQuery("Update TaxPowerCRM.dbo.Clients Set UpdationReminder=@UpdationReminder Where ClientID=@ClientID", CM => { CM.Parameters.AddWithValue("UpdationReminder", "Updation Reminder for F.Y. 2023-2024 is sent on " + DateTime.Now); CM.Parameters.AddWithValue("ClientID", Convert.ToString(Session["ClientID"])); });

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Mail send successfully.');", true);
                    }
                }
            }
            catch (Exception)
            {

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Unable to send mail. Error is: " + ex.Message + "');", true);
            }
        }

    }
}

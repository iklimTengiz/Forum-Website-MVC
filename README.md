# Forum-Website-MVC
MVC forum web site with admin panel. Basic. (bootstrap)
CONTACT FORM -------------------------------------------------------------------------
Mail and password areas must change with real mail and password.
Home Controller -> ActionResult iletisim (Contorller) >
 		mail.To.Add("MAIL"); //Kime mail gönderilecek.
                mail.From = new MailAddress("MAİL", "YOUR WEB SİTE DOMAİN", System.Text.Encoding.UTF8);
                smp.Credentials = new NetworkCredential("MAIL", "PASSWORD");


DATABASE CONNECTİON-----------------------------------------------------------------------
Web Config -> Connection String:
 <connectionStrings><add name="Your entity name" connectionString="metadata=res://*/AlenenModel.csdl|res://*/AlenenModel.ssdl|res://*/AlenenModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=YOUR SERVER;initial catalog=YOUR DB NAME;integrated security=True;persist security info=True;multipleactiveresultsets=True;application name=EntityFramework&quot;" providerName="System.Data.EntityClient" /></connectionStrings>


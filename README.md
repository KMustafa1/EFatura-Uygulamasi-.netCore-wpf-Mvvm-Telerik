# EFatura Uygulamasi
 Veritabanı sqlServer_LocalDb kullanılmıştır. Migrations ları oluşturmak için sırasıyla CLI da;
 
 #Add-Migration Initial

ve

#Update-Database

komutlarını çalıştırın.

Entegretör olarak izibiz demo hesabı kullanılmıştır. https://dev.izibiz.com.tr/
faturaları oluştururken bu biligleri göz önünde bulundurun yoksa şematron kontrölünden geçmez

 Gönderici VKN	     Gönderici Etiket	                Alıcı VKN	      Alıcı Etiket
4840847211	      urn:mail:defaultgb@izibiz.com.tr	  4840847211    	urn:mail:defaultpk@izibiz.com.tr

entegratöre iletilen faturaların durumunu görmek için
https://portaltest.izibiz.com.tr/auth/login adresine girebilirsiniz 


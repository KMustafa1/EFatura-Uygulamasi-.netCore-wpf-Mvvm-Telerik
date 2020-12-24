# EFatura Uygulamasi
 Veritabanı sqlServer_LocalDb kullanılmıştır. Migrations ları oluşturmak için sırasıyla CLI da;
 
 #Add-Migration Initial

ve

#Update-Database

komutlarını çalıştırın.

Entegretör olarak izibiz demo hesabı kullanılmıştır. https://dev.izibiz.com.tr/


Faturaları oluştururken aşağıdaki biligleri göz önünde bulundurun yoksa fatura şematron kontrölünden geçmez

 Gönderici VKN : 4840847211		     
 Gönderici Etiket	: urn:mail:defaultgb@izibiz.com.tr                
 Alıcı VKN	: 4840847211      
 Alıcı Etiket : urn:mail:defaultpk@izibiz.com.tr
      	      	
entegratöre iletilen faturaların durumunu görmek için
https://portaltest.izibiz.com.tr/auth/login adresini ziyaret edebilirsiniz 


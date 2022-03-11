<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<meta charset="UTF-8">
	<title>Php Ajax ile SweetAlert kullanarak Veri Ekleme</title>
	<link rel="stylesheet" href="sweetalert.css" />
	<script type="text/javascript" src="http://code.jquery.com/jquery-1.11.3.min.js"></script>
	<script type="text/javascript" src="sweetalert.js"></script>
	<script type="text/javascript">
		$(document).ready(function(){
			$("#KayitOl").on("click",function(){
				
				var username	=	$("#username").val();
				var password 	= 	$("#password").val();
				var email 		= 	$("#email").val();
				
				var Data 		=	"username="+username+"&password="+password+"&email="+email;
				
				$.ajax({
					type : "POST",
					url  : "ajax.php",
					data : Data,
					error : function(SendError){
						swal("HATA","Bir Hata Oluştu, Tekrar Deneyiniz.","error");
					},
					success : function(SendSuccess){
						swal("TEBRİKLER","Başarılı bir şekilde kayıt oldunuz..","success");
					}
				});
			});
		})
	</script>
</head>
<body>
	
	<form action="" onsubmit="return false;">
		<table>
			<tr>
				<td>Kullanıcı Adı : </td>
				<td><input type="text" name="username" id="username" /></td>
			</tr>
			<tr>
				<td>Şifre : </td>
				<td><input type="password" name="password" id="password" /></td>
			</tr>
			<tr>
				<td>Email Adresi : </td>
				<td><input type="email" name="email" id="email" /></td>
			</tr>
			<tr>
				<td></td>
				<td><input type="submit" value="Kayıt Ol" id="KayitOl" /></td>
			</tr>
		</table>
	</form>
	
</body>
</html>
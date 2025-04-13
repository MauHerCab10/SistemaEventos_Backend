using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Transversal.Modelos;

namespace SistemaEventos.Comun
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;

        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EncriptarContraseña(string pContrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesContrasena = sha256.ComputeHash(Encoding.UTF8.GetBytes(pContrasena));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytesContrasena.Length; i++)
                {
                    //El formato "x2" convierte cada byte del array en su representación hexadecimal. La "x" indica el formato hexadecimal y el "2" asegura que cada valor tenga al menos dos caracteres, añadiendo un cero a la izquierda si es necesario
                    sb.Append(bytesContrasena[i].ToString("x2"));
                }

                //Contraseña encriptada
                return sb.ToString();
            }
        }

        public string GenerarJWT(Usuario modelo)
        {
            //información del usuario
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, modelo.NombreUsuario)
            };

            //creación de la Llave de Seguridad
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));

            //creación de las Credenciales de seguridad
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Parametrización del Token
            var configurationJWT = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            //Token generado
            var token = new JwtSecurityTokenHandler().WriteToken(configurationJWT);

            return token;
        }

        public bool ValidarToken(string token)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false, //valida q las apps externas puedan usar la URL donde se encuentra nuestra Api
                ValidateAudience = false, //quienes pueden acceder a nuestra Api
                ValidateLifetime = true, //valida el tiempo de vida del Token
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!))
            };

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

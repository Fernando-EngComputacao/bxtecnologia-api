using BXTecnologia.API.Config.Interfaces;

namespace BXTecnologia.API.Config;

public class EmailLayout : IEmailLayout
{
    /// <summary>
    /// Generates a registration confirmation email template with a modern and futuristic design
    /// </summary>
    /// <param name="userName">Name of the user who registered</param>
    /// <param name="confirmationLink">Optional confirmation link to include in the email</param>
    /// <returns>HTML formatted email body</returns>
    public string GetUserRegistrationEmailTemplate(string userName, string? confirmationLink = null)
    {
        string confirmationButton = string.IsNullOrEmpty(confirmationLink)
            ? string.Empty
            : $@"<tr>
                <td align=""center"" style=""padding: 25px 0;"">
                    <a href=""{confirmationLink}"" style=""background-color: #4A00E0; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block; transition: all 0.3s ease; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.4);"">CONFIRMAR CADASTRO</a>
                </td>
              </tr>";

        return $@"<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirmação de Cadastro</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f9f9f9; color: #333;"">
    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" style=""max-width: 600px; margin: auto; background: linear-gradient(135deg, #ffffff 0%, #f5f5f5 100%); border-radius: 15px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 10px 30px rgba(0,0,0,0.1);"">
        <tr>
            <td style=""padding: 0;"">
                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                    <!-- Header with gradient -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); padding: 30px 40px; text-align: center;"">
                            <h1 style=""color: white; margin: 0; font-size: 28px; font-weight: 600; letter-spacing: 1px;"">BX Tecnologia</h1>
                        </td>
                    </tr>
                    
                    <!-- Main Content -->
                    <tr>
                        <td style=""padding: 40px 40px 20px;"">
                            <h2 style=""margin-top: 0; margin-bottom: 20px; color: #333; font-size: 24px; font-weight: 600;"">Cadastro Confirmado!</h2>
                            <p style=""margin-bottom: 20px; font-size: 16px; line-height: 1.6; color: #555;"">
                                Olá <strong>{userName}</strong>,
                            </p>
                            <p style=""margin-bottom: 20px; font-size: 16px; line-height: 1.6; color: #555;"">
                                Seu cadastro na BX Tecnologia foi realizado com sucesso! Estamos muito felizes em tê-lo(a) como nosso cliente.
                            </p>
                            <p style=""margin-bottom: 30px; font-size: 16px; line-height: 1.6; color: #555;"">
                                Agora você tem acesso a todos os nossos serviços e recursos exclusivos. Não hesite em entrar em contato caso precise de qualquer assistência.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Confirmation Button (if link provided) -->
                    {confirmationButton}
                    
                    <!-- Feature Icons -->
                    <tr>
                        <td style=""padding: 0 40px 40px;"">
                            <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin-top: 20px; background-color: rgba(74, 0, 224, 0.05); border-radius: 10px; padding: 20px;"">
                                <tr>
                                    <td align=""center"" style=""padding: 10px; width: 33%;"">
                                        <div style=""width: 60px; height: 60px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin: 0 auto; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.3);"">
                                            <span style=""color: white; font-size: 24px;"">🔒</span>
                                        </div>
                                        <p style=""margin-top: 10px; font-weight: 600; color: #333; font-size: 14px;"">Segurança</p>
                                    </td>
                                    <td align=""center"" style=""padding: 10px; width: 33%;"">
                                        <div style=""width: 60px; height: 60px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin: 0 auto; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.3);"">
                                            <span style=""color: white; font-size: 24px;"">⚡</span>
                                        </div>
                                        <p style=""margin-top: 10px; font-weight: 600; color: #333; font-size: 14px;"">Velocidade</p>
                                    </td>
                                    <td align=""center"" style=""padding: 10px; width: 33%;"">
                                        <div style=""width: 60px; height: 60px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin: 0 auto; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.3);"">
                                            <span style=""color: white; font-size: 24px;"">🌟</span>
                                        </div>
                                        <p style=""margin-top: 10px; font-weight: 600; color: #333; font-size: 14px;"">Inovação</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f0f0f0; padding: 30px 40px; text-align: center;"">
                            <p style=""margin: 0; font-size: 14px; color: #777; margin-bottom: 10px;"">
                                &copy; 2025 BX Tecnologia. Todos os direitos reservados.
                            </p>
                            <p style=""margin: 0; font-size: 14px; color: #777;"">
                                Este é um email automático, por favor não responda.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }

    /// <summary>
    /// Generates an image processing confirmation email template with a modern and futuristic design
    /// </summary>
    /// <param name="userName">Name of the user</param>
    /// <param name="imageCount">Number of images processed</param>
    /// <param name="viewImagesLink">Optional link to view the processed images</param>
    /// <returns>HTML formatted email body</returns>
    public string GetImageProcessingEmailTemplate(string userName, int imageCount,
        string? viewImagesLink = null)
    {
        string viewImagesButton = string.IsNullOrEmpty(viewImagesLink)
            ? string.Empty
            : $@"<tr>
                <td align=""center"" style=""padding: 25px 0;"">
                    <a href=""{viewImagesLink}"" style=""background-color: #4A00E0; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block; transition: all 0.3s ease; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.4);"">VISUALIZAR IMAGENS</a>
                </td>
              </tr>";

        string imageText = imageCount == 1
            ? "1 imagem foi processada"
            : $"{imageCount} imagens foram processadas";

        return $@"<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Processamento de Imagens Concluído</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f9f9f9; color: #333;"">
    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" style=""max-width: 600px; margin: auto; background: linear-gradient(135deg, #ffffff 0%, #f5f5f5 100%); border-radius: 15px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 10px 30px rgba(0,0,0,0.1);"">
        <tr>
            <td style=""padding: 0;"">
                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                    <!-- Header with gradient -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); padding: 30px 40px; text-align: center;"">
                            <h1 style=""color: white; margin: 0; font-size: 28px; font-weight: 600; letter-spacing: 1px;"">BX Tecnologia</h1>
                        </td>
                    </tr>
                    
                    <!-- Main Content -->
                    <tr>
                        <td style=""padding: 40px 40px 20px;"">
                            <h2 style=""margin-top: 0; margin-bottom: 20px; color: #333; font-size: 24px; font-weight: 600;"">Processamento Concluído!</h2>
                            <p style=""margin-bottom: 20px; font-size: 16px; line-height: 1.6; color: #555;"">
                                Olá <strong>{userName}</strong>,
                            </p>
                            <p style=""margin-bottom: 20px; font-size: 16px; line-height: 1.6; color: #555;"">
                                Temos boas notícias! {imageText} com sucesso e está disponível em sua conta.
                            </p>
                            <p style=""margin-bottom: 30px; font-size: 16px; line-height: 1.6; color: #555;"">
                                As imagens foram armazenadas com segurança em nosso sistema e estão prontas para uso. Você pode acessá-las a qualquer momento através de sua conta.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Image Processing Visualization -->
                    <tr>
                        <td style=""padding: 0 40px;"">
                            <div style=""background-color: rgba(74, 0, 224, 0.05); border-radius: 10px; padding: 20px; text-align: center;"">
                                <div style=""display: inline-block; position: relative; margin: 10px;"">
                                    <div style=""width: 80px; height: 80px; background: #e0e0e0; border-radius: 10px; display: inline-block; position: relative; overflow: hidden;"">
                                        <div style=""position: absolute; top: 0; left: 0; width: 100%; height: 100%; background: linear-gradient(45deg, rgba(142, 45, 226, 0.2) 0%, rgba(74, 0, 224, 0.2) 100%); display: flex; align-items: center; justify-content: center;"">
                                            <span style=""color: #4A00E0; font-size: 30px;"">📷</span>
                                        </div>
                                    </div>
                                    <div style=""position: absolute; bottom: -10px; right: -10px; width: 30px; height: 30px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; box-shadow: 0 3px 8px rgba(74, 0, 224, 0.3);"">
                                        <span style=""color: white; font-size: 16px;"">✓</span>
                                    </div>
                                </div>
                                <p style=""margin-top: 20px; font-weight: 600; color: #333; font-size: 16px;"">{imageText}</p>
                            </div>
                        </td>
                    </tr>
                    
                    <!-- View Images Button (if link provided) -->
                    {viewImagesButton}
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f0f0f0; padding: 30px 40px; text-align: center; margin-top: 30px;"">
                            <p style=""margin: 0; font-size: 14px; color: #777; margin-bottom: 10px;"">
                                &copy; 2025 BX Tecnologia. Todos os direitos reservados.
                            </p>
                            <p style=""margin: 0; font-size: 14px; color: #777;"">
                                Este é um email automático, por favor não responda.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }

    /// <summary>
    /// Generates an image registration confirmation email template with a modern and futuristic design
    /// </summary>
    /// <param name="userName">Name of the user</param>
    /// <param name="imageName">Name of the registered image</param>
    /// <param name="viewImageLink">Optional link to view the registered image</param>
    /// <returns>HTML formatted email body</returns>
    public string GetImageRegistrationTemplate(string userName, string imageName, string? viewImageLink = null)
    {
        string viewImageButton = string.IsNullOrEmpty(viewImageLink) 
            ? string.Empty 
            : $@"<tr>
                <td align=""center"" style=""padding: 25px 0;"">
                    <a href=""{viewImageLink}"" style=""background-color: #4A00E0; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block; transition: all 0.3s ease; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.4);"">VISUALIZAR IMAGEM</a>
                </td>
              </tr>";

        return $@"<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Cadastro de Imagem Confirmado</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f9f9f9; color: #333;"">
    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" style=""max-width: 600px; margin: auto; background: linear-gradient(135deg, #ffffff 0%, #f5f5f5 100%); border-radius: 15px; overflow: hidden; margin-top: 40px; margin-bottom: 40px; box-shadow: 0 10px 30px rgba(0,0,0,0.1);"">
        <tr>
            <td style=""padding: 0;"">
                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                    <!-- Header with gradient -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); padding: 30px 40px; text-align: center;"">
                            <h1 style=""color: white; margin: 0; font-size: 28px; font-weight: 600; letter-spacing: 1px;"">BX Tecnologia</h1>
                        </td>
                    </tr>
                    
                    <!-- Main Content -->
                    <tr>
                        <td style=""padding: 40px 40px 20px;"">
                            <h2 style=""margin-top: 0; margin-bottom: 20px; color: #333; font-size: 24px; font-weight: 600;"">Imagem Cadastrada com Sucesso!</h2>
                            <p style=""margin-bottom: 20px; font-size: 16px; line-height: 1.6; color: #555;"">
                                Olá <strong>{userName}</strong>,
                            </p>
                            <p style=""margin-bottom: 20px; font-size: 16px; line-height: 1.6; color: #555;"">
                                Temos boas notícias! Sua imagem <strong>{imageName}</strong> foi cadastrada com sucesso em nosso sistema.
                            </p>
                            <p style=""margin-bottom: 30px; font-size: 16px; line-height: 1.6; color: #555;"">
                                A imagem foi armazenada com segurança e está pronta para uso. Você pode acessá-la a qualquer momento através de sua conta.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Image Registration Visualization -->
                    <tr>
                        <td style=""padding: 0 40px;"">
                            <div style=""background-color: rgba(74, 0, 224, 0.05); border-radius: 10px; padding: 20px; text-align: center;"">
                                <div style=""display: inline-block; position: relative; margin: 10px;"">
                                    <div style=""width: 100px; height: 100px; background: #e0e0e0; border-radius: 10px; display: inline-block; position: relative; overflow: hidden;"">
                                        <div style=""position: absolute; top: 0; left: 0; width: 100%; height: 100%; background: linear-gradient(45deg, rgba(142, 45, 226, 0.2) 0%, rgba(74, 0, 224, 0.2) 100%); display: flex; align-items: center; justify-content: center;"">
                                            <span style=""color: #4A00E0; font-size: 36px;"">🖼️</span>
                                        </div>
                                    </div>
                                    <div style=""position: absolute; bottom: -10px; right: -10px; width: 35px; height: 35px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; box-shadow: 0 3px 8px rgba(74, 0, 224, 0.3);"">
                                        <span style=""color: white; font-size: 18px;"">✓</span>
                                    </div>
                                </div>
                                <p style=""margin-top: 20px; font-weight: 600; color: #333; font-size: 16px;"">{imageName}</p>
                            </div>
                        </td>
                    </tr>
                    
                    <!-- View Image Button (if link provided) -->
                    {viewImageButton}
                    
                    <!-- Features Section -->
                    <tr>
                        <td style=""padding: 30px 40px 40px;"">
                            <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin-top: 20px; background-color: rgba(74, 0, 224, 0.05); border-radius: 10px; padding: 20px;"">
                                <tr>
                                    <td align=""center"" style=""padding: 10px; width: 33%;"">
                                        <div style=""width: 60px; height: 60px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin: 0 auto; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.3);"">
                                            <span style=""color: white; font-size: 24px;"">🔒</span>
                                        </div>
                                        <p style=""margin-top: 10px; font-weight: 600; color: #333; font-size: 14px;"">Segurança</p>
                                    </td>
                                    <td align=""center"" style=""padding: 10px; width: 33%;"">
                                        <div style=""width: 60px; height: 60px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin: 0 auto; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.3);"">
                                            <span style=""color: white; font-size: 24px;"">⚡</span>
                                        </div>
                                        <p style=""margin-top: 10px; font-weight: 600; color: #333; font-size: 14px;"">Velocidade</p>
                                    </td>
                                    <td align=""center"" style=""padding: 10px; width: 33%;"">
                                        <div style=""width: 60px; height: 60px; background: linear-gradient(135deg, #8E2DE2 0%, #4A00E0 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin: 0 auto; box-shadow: 0 5px 15px rgba(74, 0, 224, 0.3);"">
                                            <span style=""color: white; font-size: 24px;"">🌟</span>
                                        </div>
                                        <p style=""margin-top: 10px; font-weight: 600; color: #333; font-size: 14px;"">Qualidade</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f0f0f0; padding: 30px 40px; text-align: center; margin-top: 30px;"">
                            <p style=""margin: 0; font-size: 14px; color: #777; margin-bottom: 10px;"">
                                &copy; 2025 BX Tecnologia. Todos os direitos reservados.
                            </p>
                            <p style=""margin: 0; font-size: 14px; color: #777;"">
                                Este é um email automático, por favor não responda.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }
}
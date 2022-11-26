using FreedomLogic.Resources;
using System.ComponentModel.DataAnnotations;

namespace FreedomWeb.ViewModels.Errors
{
    public enum ErrorCode
    {
        [Display(Name = "ErrDefault", ResourceType = typeof(ErrorRes))]
        ErrDefault = 0,

        [Display(Name = "ErrBadRequest", ResourceType = typeof(ErrorRes))]
        BadRequest = 400,

        [Display(Name = "ErrUnauthorized", ResourceType = typeof(ErrorRes))]
        Unauthorized = 401,

        [Display(Name = "ErrNotFound", ResourceType = typeof(ErrorRes))]
        NotFound = 404,

        [Display(Name = "ErrInternalServer", ResourceType = typeof(ErrorRes))]
        InternalServerError = 500
    }

    public class ErrorViewModel
    {
        public ErrorCode Error { get; set; }
    }
}
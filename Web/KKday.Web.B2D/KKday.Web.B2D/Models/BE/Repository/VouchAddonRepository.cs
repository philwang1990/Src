using System;
using KKday.Web.B2D.BE.AppCode.DAL.Voucher;
using KKday.Web.B2D.BE.Models.Model.Voucher;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class VouchAddonRepository
    {
        public B2dVoucherAddon GetVoucherAddon(Int64 comp_xid)
        {
            return VoucherAddonDAL.GetVoucherAddon(comp_xid);
        }

        public void UpdateVouchAddon(B2dVoucherAddon addon, string upd_user)
        {
            VoucherAddonDAL.UpdateVouchAddon(addon, upd_user);
        }

        public void UpdateVoucheLogo(Int64 comp_xid, string logo_url, string upd_user)
        {
            VoucherAddonDAL.UpdateVoucheLogo(comp_xid, logo_url, upd_user);
        }
    }
}

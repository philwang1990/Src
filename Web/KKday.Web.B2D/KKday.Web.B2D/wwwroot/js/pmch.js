//刷新套餐區塊




function toSuccess()
{
  var newForm = $('<form>', {
                       'action': "https://localhost:5001/Final/Step3/18KK112074910",
                       'target': '_self',
                       'method': 'post'
                   }).append(jQuery('<input>', {
                       'name': 'jsondata',
                       'id' :'jsondata',
                       'value':JSON.stringify({"metadata":{"status":"0000","desc":""},"data":{"pmgw_trans_no":"U2FsdGVkX1/bo/sZkGkaxiMsYa39gnPWk66gq3lSFzQ=","is_3d":false,"pmgw_method":"AUTH","transaction_code":"013604","pay_currency":"TWD","pay_amount":263,"is_fraud":0,"risk_note":"","member_info":{"encode_card_no":"************2782"}}}),
                       'type': 'hidden'
                       }));
    $(document.body).append(newForm);
                   newForm.submit();

}


function toSuccess2() {

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Final/Success/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({"isSuccess":true,"errorCode":"","errorMsg":"","pmgwTransNo":"PMGW000000000","pmgwMethod":"AUTH","transactionCode":"76767","payCurrency":"TWD","payAmount":210.0,"is3D":false,"memberInfo":{"encodeCardNo":"3337"},"isFraud":"0","riskNote":""}),
        dataType: "text",
        cache: false,
        async: true,
        //timeout: 60000,
        error: function (jqXHR, textStatus, errorThrown) {
            alert('要改');
        },
        success: function () {
            alert('要改2');
        },
        complete: function () {
             

 
        }
    });
}

//刷新套餐區塊




function toSuccess()
{
  var newForm = $('<form>', {
                       'action': "https://localhost:5001/Final/Success/18KK102673551",
                       'target': '_self',
                       'method': 'post'
                   }).append(jQuery('<input>', {
                       'name': 'jsondata',
                       'id' :'jsondata',
                       'value':JSON.stringify({"isSuccess":true,"pmgwTransNo":"U2FsdGVkX19X8zoD1GYITT+shtmeku9z+0tfxvONi2w=","pmgwMethod":"AUTH","transactionCode":"007558","payCurrency":"TWD","payAmount":"263","is3D":false,"isFraud":"0","riskNote":"","memberInfo":{"encodeCardNo":"************3617"}}),
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

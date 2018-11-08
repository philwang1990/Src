


//////

function UpdateCompany()
{
   if(!confirm('Are you sure to update company profile?')) return false;

   var country_area =  $("#country_area").val().split(',');
   var cmpObj = { xid : $("#xid").val(),
        status : $("#status").val(),
        coop_mode :$("#coop_mode").val(),
        payment_type :$("#pay_type").val(),
        name :$("#name").val(),
        url :$("#url").val(),
        locale :$("#locale").val(),
        currency :$("#currency").val(),
        invoice :$("#invoice").val(),
        country : country_area[1],
        tel_country_code : country_area[0],
        tel :$("#tel").val(),
        address :$("#address").val(),
        contact_user :$("#contact").val(),
        contact_user_email :$("#contact_email").val(),
        finance_user :$("#finance").val(),
        sales_user :$("#sales").val()
    };

    console.log("company: " + JSON.stringify(cmpObj));

    $.ajax({
        type: "POST",
        url: _root_path + "Company/Update",
        contentType: "application/json",
        data: JSON.stringify(cmpObj),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) {
            // console.log("result: " + JSON.stringify(result));
            if(result.status == "OK") {
                alert("Update OK!");

                var newForm = $('<form>', {
                    'action': _root_path + "Company",
                    'target': '_self',
                    'method': 'post'
                }).append(jQuery('<input>', {
                    'name': 'query',
                    'value': /query=([^&#=]*)/.exec(window.location.search)[1],
                    'type': 'hidden'
                }));
                newForm.submit();
            } else {
                alert("Error: " + result.msg);
            }
        }
    });

    return false;
}

function SetStatus(code) {
    $.ajax({
        type: "POST",
        url: _root_path + "Company/SetStatus",
        contentType: "application/json",
        data: JSON.stringify({ xid :$("#xid").val(), status : code }),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) {
            if(result.status == "OK") {
                var newForm = $('<form>', {
                    'action': _root_path + "Company",
                    'target': '_self',
                    'method': 'post'
                }).append(jQuery('<input>', {
                    'name': 'query',
                    'value': /query=([^&#=]*)/.exec(window.location.search)[1],
                    'type': 'hidden'
                }));
                newForm.submit();
            } else {
                alert("Error: " + result.msg);
            }
        }
    });

    return false;
}

function UploadLogo() {

    if($('#logo_file').val() == "") {
        alert('請先選擇要上傳的Logo圖檔!');
        return false;
    } 
 
    var input = document.getElementById('logo_file');
    var files = input.files;
    var formData = new FormData();

    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]); 
    }

    $.ajax({
        url: _root_path + "Company/UploadLogo/?cid=" + $("#xid").val(),
        type: 'POST',
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) {
            if(result.status == "OK") {
                alert('Upload logo file is Successful');

                $("#logo_file").prev('div').html(result.img_url[0]);
            }
            else  alert(result.msg);               
        }

    });
}

 function UploadLicenses() {
    var formData = new FormData();

    if($('#license_files').val() == "") {
        alert('請先選擇要上傳的營業登記檔案!');
        return false;
    }
 
    var input = document.getElementById('license_files');
    var files = input.files;

    if(files.length > 2) {
        alert('只可上傳最多 2 個營業登記檔案!');
        return false;
    }

    var formData = new FormData();

    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]); 
    }

    $.ajax({
        url: _root_path + "Company/UploadLicenses/?cid=" + $("#xid").val(),
        type: 'POST',
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) { 
            // console.log(JSON.stringify(result)); 
            if(result.status == "OK") {
                alert('Upload license files is Successful');  
                $("#license_files").prev('div').html(result.img_url[0] + ((result.img_url.length > 1) ? "<br/>" + result.img_url[1] : ""));
            }
            else  alert(result.msg);               
        }

    });
}

function UpdateVouch() {
    var vouchData = { 
        company_xid: $("#xid").val(), 
        company_name: $("#vao_cmp_name").val(), 
        address: $("#vao_cmp_addr").val(), 
        tel : $("#vao_tel").val(),
        email: $("#vao_email").val()
    };

    $.ajax({
        url: _root_path + "Company/UpdateVouchAddon",
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(vouchData),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) {
            if(result.status == "OK") {
                alert('Upload Voucher Add-on is Successful');
            }
            else  alert(result.msg);               
        }

    });
}
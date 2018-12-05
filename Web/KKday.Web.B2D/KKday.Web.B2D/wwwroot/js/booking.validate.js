$.validator.addMethod("othImei", function (value, element) {
           if($("#txtOtherImei").is(":visible")==true)
           {
             if ( value.match(/^([0-9]){15,}$/))
              {return true;}else{
              return false; }
           }
           else
           {
             return true;
           }
          }, $("#booking_step1_length_error_1").val()+"15"+ $("#booking_step1_length_error_2").val());


$.validator.addMethod("sendSendDataBuyerPassfName", function (value, element) {
           if($("#txtSendDataBuyerPassfName").is(":visible")==true)
           {
             if ( value.match(/^[A-Za-z]+(( |-){1}[a-zA-Z]+)*$/))
              {return true;}else{
              return false; }
           }
           else
           {
             return true;
           }
          }, $("#booking_step1_english_error").val());

$.validator.addMethod("sendSendDataBuyerPasslName", function (value, element) {
           if($("#txtSendDataBuyerPasslName").is(":visible")==true)
           {
             if ( value.match(/^[A-Za-z]+(( |-){1}[a-zA-Z]+)*$/))
              {return true;}else{
              return false; }
           }
           else
           {
             return true;
           }
}, $("#booking_step1_english_error").val());


$.validator.addMethod("contactfName", function (value, element) {
           if($("#txtContactfName").is(":visible")==true)
           {
             if ( value.match(/^[A-Za-z]+(( |-){1}[a-zA-Z]+)*$/))
              {return true;}else{
              return false; }
           }
           else
           {
             return true;
           }
}, $("#booking_step1_english_error").val());


$.validator.addMethod("contactlName", function (value, element) {
           if($("#txtContactlName").is(":visible")==true)
           {
             if ( value.match(/^[A-Za-z]+(( |-){1}[a-zA-Z]+)*$/))
              {return true;}else{
              return false; }
           }
           else
           {
             return true;
           }
}, $("#booking_step1_english_error").val());


$.validator.addMethod("payCardNumber", function (value, element) {
           if($("#txtPayCardNum").is(":visible")==true)
           {
             if ( value.replace(" ","").replace(" ","").replace(" ","").match(/^([0-9]){16}$/))
              {return true;}else{
              return false; }
           }
           else
           {
             return true;
           }
          }, $("#booking_step1_length_error_1").val()+"16"+ $("#booking_step1_length_error_2").val());


function initModule2()
{
    $("#form1").validate({
        rules: {
            txtLocalFname: "required",
            txtLocalLname: "required",
            txtNationality : {required : $("#txtNationality").is(":visible")==true} 
        },
        messages: {
            txtLocalFname: $("#booking_step1_required_error").val(),
            txtLocalLname: $("#booking_step1_required_error").val(),
            txtNationality :$("#booking_step1_required_error").val()
            },
        errorClass: "error_msg"
    });

    formVaildate(); 
}

function ReParseValidation() {
    var forms = $("body").find("form");
    for (var i = 0; i < forms.length; i++) {
        var form = $(forms[i]);

        // 移除所有的檢核設定
        form.removeData("validator")
            .removeData("unobtrusiveValidation");

        // 重新解析元素的data-val-*標示
        //$.validator.unobtrusive.parse(form);
    }
}


function chkValid(e)
{
   $(e).prop('disabled', true);

   $("#board1").removeClass("active").addClass("active");
   $("#board2").removeClass("active").addClass("active");
   $("#board3").removeClass("active").addClass("active");

   var payRadio= $('input:radio[name="payment"]:checked').val();
   if(payRadio==null){  $(e).prop('disabled', false); return false; }


   ReParseValidation();
   formVaildate();

    var chkchk=true;
   $(".formClass").each(function () {
   
         var id =$(this).attr("id");
         if($("#"+id).valid()==false) 
         {
            chkchk=false; 
         }
    });

  $(".otherFormClass").each(function () {
   
         var id =$(this).attr("id");
         if($("#"+id).valid()==false) 
         {
            chkchk=false; 
         }
    });

  $(".payFormClass").each(function () {
   
         var id =$(this).attr("id");
         if($("#"+id).valid()==false) 
         {
            chkchk=false; 
         }
    });

   if(chkchk==false) 
   {
       $(e).prop('disabled', false);
       return false;
   }

  setdataModel();
  toStep1();
}


//booking step1
function toStep1() {

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Booking/bookingStep1/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataModel) ,
        dataType: "json",
        cache: false,
        async: true,
        //timeout: 60000,
        error: function (jqXHR, textStatus, errorThrown) {
                 location.href =_root_path + "Final/Failure/"
        },
        success: function (result) {if(result.status=="OK"){
               sendPayment(result.pmchSslRequest,result.url);
 
            } else{
                 alert("booking-err:"+result.msgErr);
                 location.href =_root_path + "Home/"
            };
        },
        complete: function () {
        }
    });
}

function sendPayment(callPmchReq,url)
{
    var newForm = $('<form>', {
                       'action': url,
                       'target': '_self',
                       'method': 'post'
                   }).append(jQuery('<input>', {
                       'name': 'jsondata',
                       'id' :'jsondata',
                       'value':JSON.stringify(callPmchReq),
                       'type': 'hidden'
                       }));
    $(document.body).append(newForm);
                   newForm.submit();

}


function formVaildate()
{
   $(".formClass").each(function () {

         var id =$(this).attr("id");
         var i = id.split('_')[1];

         $(this).validate({
                rules: {
                    txtEngLast: {required : $("#txtEngLast_"+i).is(":visible")==true},
                    txtEngFirst: {required : $("#txtEngFirst_"+i).is(":visible")==true},
                    selGender: {required : $("#selGender_"+i).is(":visible")==true},
                    txtBirtyday:  {required : $("#txtBirtyday_"+i).is(":visible")==true},
                    selcusCountry:  {required : $("#selcusCountry_"+i).is(":visible")==true},
                    txtPassportId: {required : $("#txtPassportId_"+i).is(":visible")==true},
                    txtPassDate: {required : $("#txtPassDate_"+i).is(":visible")==true},
                    txtlocalFirstName: {required : $("#txtlocalFirstName_"+i).is(":visible")==true},
                    txtlocalLastName: {required : $("#txtlocalLastName_"+i).is(":visible")==true},
                    txtNationTW: {required : $("#txtNationTW_"+i).is(":visible")==true},
                    txtNationHKMO: {required : $("#txtNationHKMO_"+i).is(":visible")==true},
                    txtNationMTP: {required : $("#txtNationMTP_"+i).is(":visible")==true},
                    txtHigh: {required : $("#txtHigh_"+i).is(":visible")==true},
                    selHighUnit: {required : $("#selHighUnit_"+i).is(":visible")==true},
                    txtWeight: {required : $("#txtWeight_"+i).is(":visible")==true},
                    selWeightUnit: {required : $("#selWeightUnit_"+i).is(":visible")==true},
                    selSize: {required : $("#selSize_"+i).is(":visible")==true},
                    selShoeUnitM: {required : $("#selShoeUnitM_"+i).is(":visible")==true},
                    selShoeUnitW: {required : $("#selShoeUnitW_"+i).is(":visible")==true},
                    selShoeUnitC: {required : $("#selShoeUnitC_"+i).is(":visible")==true},
                    selShoeSizeM: {required : $("#selShoeSizeM_"+i).is(":visible")==true},
                    selShoeSizeW: {required : $("#selShoeSizeW_"+i).is(":visible")==true},
                    selShoeSizeC: {required : $("#selShoeSizeC_"+i).is(":visible")==true},
                    selGlass: {required : $("#selGlass_"+i).is(":visible")==true}
                },
                messages: {
                    txtEngLast: {required : $("#booking_step1_required_error").val()},
                    txtEngFirst: {required : $("#booking_step1_required_error").val()},
                    selGender: {required : $("#booking_step1_required_error").val()},
                    txtBirtyday: {required : $("#booking_step1_required_error").val()},
                    selcusCountry: {required : $("#booking_step1_required_error").val()},
                    txtPassportId: {required : $("#booking_step1_required_error").val()},
                    txtPassDate: {required : $("#booking_step1_required_error").val()},
                    txtlocalFirstName: {required : $("#booking_step1_required_error").val()},
                    txtlocalLastName: {required : $("#booking_step1_required_error").val()},
                    txtNationTW: {required : $("#booking_step1_required_error").val()},
                    txtNationHKMO: {required : $("#booking_step1_required_error").val()},
                    txtNationMTP: {required : $("#booking_step1_required_error").val()},
                    txtHigh: {required : $("#booking_step1_required_error").val()},
                    selHighUnit: {required : $("#booking_step1_required_error").val()},
                    txtWeight: {required : $("#booking_step1_required_error").val()},
                    selWeightUnit :{required : $("#booking_step1_required_error").val()},
                    selSize :{required : $("#booking_step1_required_error").val()},
                    selShoeUnitM :{required : $("#booking_step1_required_error").val()},
                    selShoeUnitW :{required : $("#booking_step1_required_error").val()},
                    selShoeUnitC :{required : $("#booking_step1_required_error").val()},
                    selShoeSizeM :{required : $("#booking_step1_required_error").val()},
                    selShoeSizeW :{required : $("#booking_step1_required_error").val()},
                    selShoeSizeC :{required : $("#booking_step1_required_error").val()},
                    selGlass :{required : $("#booking_step1_required_error").val()},
                    selMealType:{required : $("#booking_step1_required_error").val()}
                    },
                errorClass: "has-error",
                errorPlacement: function(error, element) { 
                  if($(element).attr("name")=="txtBirtyday" || $(element).attr("name")=="txtPassDate")
                  {
                    error.appendTo(element.parent().parent());  
                  }
                  else
                  {
                     error.appendTo(element.parent());  
                  }
               }
               
            });
        });


     $(".otherFormClass").each(function () {
    
          $(this).validate({
                rules: {
                    txtOtherModleNo: {required : $("#txtOtherActDate").is(":visible")==true},
                    txtOtherImei:  { othImei :true ,required : $("#txtOtherImei").is(":visible")==true },
                    selOtherLocation :{required : $("#selOtherLocation").is(":visible")==true},
                    txtSendDataRcefName :{required : $("#txtSendDataRcefName").is(":visible")==true},
                    txtSendDataRcelName :{required : $("#txtSendDataRcelName").is(":visible")==true},
                    selSendDataCountryCode :{required : $("#selSendDataCountryCode").is(":visible")==true},
                    selSendDataCityCode :{required : $("#selSendDataCityCode").is(":visible")==true},
                    txtSendDataZipCode :{required : $("#txtSendDataZipCode").is(":visible")==true},
                    txtSendDataReceiverTel :{required : $("#txtSendDataReceiverTel").is(":visible")==true},
                    txtSendDataHtlName :{required : $("#txtSendDataHtlName").is(":visible")==true},
                    txtSendDataHtlTel :{required : $("#txtSendDataHtlTel").is(":visible")==true},
                    txtSendDataHtlAddress :{required : $("#txtSendDataHtlAddress").is(":visible")==true},
                    txtSendDataBuyerPassfName :{sendSendDataBuyerPassfName :true ,required : $("#txtSendDataBuyerPassfName").is(":visible")==true},
                    txtSendDataBuyerPasslName :{sendSendDataBuyerPasslName :true ,required : $("#txtSendDataBuyerPasslName").is(":visible")==true},
                    txtSendDataBuyerLocalfName :{required : $("#txtSendDataBuyerLocalfName").is(":visible")==true},
                    txtSendDatBuyerLocallName :{required : $("#txtSendDatBuyerLocallName").is(":visible")==true},
                    txtSendDataBookingWebSite :{required : $("#txtSendDataBookingWebSite").is(":visible")==true},
                    txtSendDataBookingOrdNo :{required : $("#txtSendDataBookingOrdNo").is(":visible")==true},
                    txtSendDataChkinDate :{required : $("#txtSendDataChkinDate").is(":visible")==true},
                    txtSendDataChkoutDate :{required : $("#txtSendDataChkoutDate").is(":visible")==true},
                    txtContactfName :{contactfName: true,required : $("#txtContactfName").is(":visible")==true},
                    txtContactlName :{contactfName:true,required : $("#txtContactlName").is(":visible")==true},
                    contact_phone :{required : $("#rdoContactPhone1").is(":visible")==true},
                    txtContactPhone :{required : $("#rdoContactPhone1").is(":visible")==true && $("#txtContactPhone").is(":visible")==true},
                    contact_app :{required : $("#rdoContactApp1").is(":visible")==true},
                    selContactApp :{required : $("#selContactApp").is(":visible")==true},
                    txtContactAppAccount :{required : $("#txtContactAppAccount").is(":visible")==true},
                    selGuide :{required : $("#selGuide").is(":visible")==true},
                    selRentCarPickupOfiice :{required : $("#selRentCarPickupOfiice").is(":visible")==true},
                    selRentCarPickUpHour :{required : $("#selRentCarPickUpHour").is(":visible")==true},
                    selRentCarPickUpMinute :{required : $("#selRentCarPickUpMinute").is(":visible")==true},
                    rentCarwifi :{required : $("#rdoGpsTrue").is(":visible")==true},
                    rentCargps :{required : $("#rdoGpsTrue").is(":visible")==true},
                    selRentCarDropOffOfiice :{required : $("#selRentCarDropOffOfiice").is(":visible")==true},
                    txtRendCarPickUpDate :{required : $("#txtRendCarPickUpDate").is(":visible")==true},
                    selRentCarPickUpHour :{required : $("#selRentCarPickUpHour").is(":visible")==true},
                    selRentCarPickUpMinute :{required : $("#selRentCarPickUpMinute").is(":visible")==true},
                    txtShttleDate:{required : $("#txtShttleDate").is(":visible")==true},
                    selShuttlePickUpTime:{required : $("#selShuttlePickUpTime").is(":visible")==true},
                    selShuttleCusHour:{required : $("#selShuttleCusHour").is(":visible")==true},
                    selShuttleCusMinute:{required : $("#selShuttleCusMinute").is(":visible")==true},
                    txtShuttlePickUpLocation:{required : $("#txtShuttlePickUpLocation").is(":visible")==true},
                    txtShuttleDropOffLocation:{required : $("#txtShuttleDropOffLocation").is(":visible")==true},
                    selShuttleLocationId:{required : $("#txtShttleDate").is(":visible")==true},
                    selShuttleCharterRoute:{required : $("#selShuttleCharterRoute").is(":visible")==true},
                    selArrFlightType:{required : $("#selArrFlightType").is(":visible")==true},
                    selArrAirport:{required : $("#selArrAirport").is(":visible")==true},
                    txtArrTerminalNo:{required : $("#txtArrTerminalNo").is(":visible")==true},
                    txtArrAirport:{required : $("#txtArrAirport").is(":visible")==true},
                    txtArrFlightNo:{required : $("#txtArrFlightNo").is(":visible")==true},
                    txtArrDate:{required : $("#txtArrDate").is(":visible")==true},
                    selArrHour:{required : $("#selArrHour").is(":visible")==true},
                    selArrMinute:{required : $("#selArrMinute").is(":visible")==true},
                    VisaType:{required : $("#rdVisaTypeTrue").is(":visible")==true},
                    selDepFlightType:{required : $("#selDepFlightType").is(":visible")==true},
                    selDepAirport:{required : $("#selDepAirport").is(":visible")==true},
                    txtDepTerminalNo:{required : $("#txtDepTerminalNo").is(":visible")==true},
                    txtDepAirport:{required : $("#txtDepAirport").is(":visible")==true},
                    txtDepFlightNo:{required : $("#txtDepFlightNo").is(":visible")==true},
                    txtDepDate:{required : $("#txtDepDate").is(":visible")==true},
                    selDepHour:{required : $("#selDepHour").is(":visible")==true},
                    selDepMinute:{required : $("#selDepMinute").is(":visible")==true},
                    selEvent1: {required : $("#selEvent1").is(":visible")==true && $("#selEvent1").is(":disabled")==false},
                    selEvent2: {required : $("#selEvent2").is(":visible")==true && $("#selEvent2").is(":disabled")==false},
                    selEvent3: {required : $("#selEvent3").is(":visible")==true && $("#selEvent3").is(":disabled")==false}

               },
                messages: {
                    txtOtherModleNo: $("#booking_step1_required_error").val(),
                    txtOtherImei: {required:$("#booking_step1_required_error").val()} ,
                    selOtherLocation :{required : $("#booking_step1_required_error").val()},
                    txtSendDataRcefName :{required : $("#booking_step1_required_error").val()},
                    txtSendDataRcelName :{required : $("#booking_step1_required_error").val()},
                    selSendDataCountryCode :{required : $("#booking_step1_required_error").val()},
                    selSendDataCityCode :{required : $("#booking_step1_required_error").val()},
                    txtSendDataZipCode :{required : $("#booking_step1_required_error").val()},
                    txtSendDataReceiverTel :{required : $("#booking_step1_required_error").val()},
                    txtSendDataHtlName :{required : $("#booking_step1_required_error").val()},
                    txtSendDataHtlTel :{required : $("#booking_step1_required_error").val()},
                    txtSendDataHtlAddress :{required : $("#booking_step1_required_error").val()},
                    txtSendDataBuyerPassfName :{required : $("#booking_step1_required_error").val()},
                    txtSendDataBuyerPasslName :{required : $("#booking_step1_required_error").val()},
                    txtSendDataBuyerLocalfName :{required : $("#booking_step1_required_error").val()},
                    txtSendDatBuyerLocallName :{required : $("#booking_step1_required_error").val()},
                    txtSendDataBookingWebSite :{required : $("#booking_step1_required_error").val()},
                    txtSendDataBookingOrdNo :{required : $("#booking_step1_required_error").val()},
                    txtSendDataChkinDate :{required : $("#booking_step1_required_error").val()},
                    txtSendDataChkoutDate :{required : $("#booking_step1_required_error").val()},
                    txtContactfName :{required : $("#booking_step1_required_error").val()},
                    txtContactlName :{required : $("#booking_step1_required_error").val()},
                    contact_phone:{required : $("#booking_step1_required_error").val()},
                    txtContactPhone:{required : $("#booking_step1_required_error").val()},
                    contact_app:{required : $("#booking_step1_required_error").val()},
                    selContactApp:{required : $("#booking_step1_required_error").val()},
                    txtContactAppAccount:{required : $("#booking_step1_required_error").val()},
                    selGuide :{required : $("#booking_step1_required_error").val()},
                    selRentCarPickupOfiice :{required : $("#booking_step1_required_error").val()},
                    selRentCarPickUpHour :{required : $("#booking_step1_required_error").val()},
                    selRentCarPickUpMinute :{required : $("#booking_step1_required_error").val()},
                    rentCarwifi :{required : $("#booking_step1_required_error").val()},
                    rentCargps :{required : $("#booking_step1_required_error").val()},
                    selRentCarDropOffOfiice :{required : $("#booking_step1_required_error").val()},
                    txtRendCarPickUpDate :{required : $("#booking_step1_required_error").val()},
                    selRentCarPickUpHour :{required : $("#booking_step1_required_error").val()},
                    selRentCarPickUpMinute :{required : $("#booking_step1_required_error").val()},
                    txtShttleDate :{required : $("#booking_step1_required_error").val()},
                    selShuttlePickUpTime :{required : $("#booking_step1_required_error").val()},
                    selShuttleCusHour :{required : $("#booking_step1_required_error").val()},
                    selShuttleCusMinute :{required : $("#booking_step1_required_error").val()},
                    txtShuttlePickUpLocation :{required : $("#booking_step1_required_error").val()},
                    txtShuttleDropOffLocation :{required : $("#booking_step1_required_error").val()},
                    selShuttleLocationId:{required : $("#booking_step1_required_error").val()},
                    selShuttleCharterRoute:{required : $("#booking_step1_required_error").val()},
                    selArrFlightType:{required : $("#booking_step1_required_error").val()},
                    selArrAirport:{required : $("#booking_step1_required_error").val()},
                    txtArrTerminalNo:{required : $("#booking_step1_required_error").val()},
                    txtArrAirport:{required : $("#booking_step1_required_error").val()},
                    txtArrFlightNo:{required : $("#booking_step1_required_error").val()},
                    txtArrDate:{required : $("#booking_step1_required_error").val()},
                    selArrHour:{required : $("#booking_step1_required_error").val()},
                    selArrMinute:{required : $("#booking_step1_required_error").val()},
                    VisaType:{required : $("#booking_step1_required_error").val()},
                    selDepFlightType:{required : $("#booking_step1_required_error").val()},
                    selDepAirport:{required : $("#booking_step1_required_error").val()},
                    txtDepTerminalNo:{required : $("#booking_step1_required_error").val()},
                    txtDepAirport:{required : $("#booking_step1_required_error").val()},
                    txtDepFlightNo:{required : $("#booking_step1_required_error").val()},
                    txtDepDate:{required : $("#booking_step1_required_error").val()},
                    selDepHour:{required : $("#booking_step1_required_error").val()},
                    selDepMinute:{required : $("#booking_step1_required_error").val()},
                    selEvent1 :{required : $("#booking_step1_required_error").val()},
                    selEvent2 :{required : $("#booking_step1_required_error").val()},
                    selEvent3 :{required : $("#booking_step1_required_error").val()}

                    },
                errorClass: "has-error",
                errorPlacement: function(error, element) {  
                  if($(element).attr("name")=="txtSendDataChkinDate" || 
                     $(element).attr("name")=="txtSendDataChkoutDate" ||
                     $(element).attr("name")=="txtShttleDate" ||    
                     $(element).attr("name")=="txtRendCarPickUpDate" ||
                     $(element).attr("name")=="txtArrDate" ||
                     $(element).attr("name")=="txtSendDataChkoutDate" )
                  {
                    error.appendTo(element.parent().parent());  
                  }
                  else
                  {
                     error.appendTo(element.parent());  
                  }
               }
            });
     });


    $(".payFormClass").each(function () {
     
          $(this).validate({
                rules: {
                    txtPayHolderName: {required : $("#txtPayHolderName").is(":visible")==true},
                    txtPayCardNum:  { payCardNumber :true ,required : $("#txtPayCardNum").is(":visible")==true },
                    txtPayExpireDate :{required : $("#txtPayExpireDate").is(":visible")==true},
                    txtPayCvc :{required : $("#txtPayCvc").is(":visible")==true}
               },
                messages: {
                    txtPayHolderName: $("#booking_step1_required_error").val(),
                    txtPayCardNum: {required:$("#booking_step1_required_error").val()} ,
                    txtPayExpireDate :{required : $("#booking_step1_required_error").val()},
                    txtPayCvc :{required : $("#booking_step1_required_error").val()}
                    },
                errorClass: "has-error"
            });
        });
        
}
function initModule()
{
    //javascript obj;
    dataModel = JSON.parse($("#hdndataModelStr").val());
    cusModel =dataModel.travelerData;
    useDate = formatDate($("#hdnUseDate").val());
    
    DateSetting();
    
}

function DateSetting()
{
    showContatPhone('2');
    showContactApp('2');
        
    if($("#selCarPsgrAdtQty").is(":visible")==true) $("#selCarPsgrAdtQty").val($("#hdnAdultQty").val());
    if($("#selCarPsgrChdQty").is(":visible")==true) $("#selCarPsgrChdQty").val($("#hdnChildQty").val());
    if($("#selCarPsgrInfQty").is(":visible")==true) $("#selCarPsgrInfQty").val($("#hdnInfantQty").val());

    var useDateTemp = new Date(useDate);
    $(".divCusBirthday").each(function() {
        var sAge = parseInt($(this).find("[id^=hdnBirthdaySage_]").val());
        var eAge = parseInt($(this).find("[id^=hdnBirthdayEage_]").val());

        $(this).find("[id^=txtBirtyday_]").daterangepicker({ 
         autoUpdateInput: false,
         singleDatePicker: true,
         showDropdowns: true,
         locale: {
            format: 'YYYY-MM-DD'
         },
         minDate :new Date(useDateTemp.getFullYear() - eAge, useDateTemp.getMonth(), useDateTemp.getDate()),
         maxDate :new Date(useDateTemp.getFullYear() - sAge, useDateTemp.getMonth(), useDateTemp.getDate()),
         startDate: new Date(useDateTemp.getFullYear() - sAge, useDateTemp.getMonth(), useDateTemp.getDate())
       });

        $(this).find("[id^=txtBirtyday_]").on('apply.daterangepicker', function(ev, picker) { 
         $(this).val(picker.startDate.format('YYYY-MM-DD'));
    });
   });
    

    $(".date-info1").daterangepicker({ 
         autoUpdateInput: false,
         singleDatePicker: true,
         showDropdowns: true,
         locale: {
            format: 'YYYY-MM-DD'
         },
    });

    $(".date-info1").on('apply.daterangepicker', function(ev, picker) { 
        $(this).val(picker.startDate.format('YYYY-MM-DD'));
    });
   

    var cutOfDay =$("#hdnCutOfDay").val();
    //send
    $(".date-info2").daterangepicker({ 
         autoUpdateInput: false,
         singleDatePicker: true,
         showDropdowns: true,
         locale: {
            format: 'YYYY-MM-DD'
         },
         minDate: moment().add(cutOfDay, 'days')
    });

    $(".date-info2").on('apply.daterangepicker', function(ev, picker) { 
        $(this).val(picker.startDate.format('YYYY-MM-DD'));
    });
    
    if($("#hdnIsEvent").val()=="Y" && $("#hdnIsHl").val()=="Y")
    {
        var dateArr = [''];
        dateArr = ($("#hdnPkgCanUseDate").val()).split(",");
        
       $(".eventDatePicker").daterangepicker({
        autoUpdateInput: false,
        minDate: moment().add(cutOfDay, 'days'), 
        singleDatePicker: true,
        showDropdowns: true, 
        "opens": "center",
        locale: {
            format: 'YYYY-MM-DD'
        },
        isInvalidDate: function(date) {
            //查不到即為銷售日
            if (dateArr.indexOf(date.format('YYYY-MM-DD')) == -1) {
                return true;
            }
        }
       });
       
       $("#txtEventDate1").on('apply.daterangepicker', function(ev, picker) { 
        $(this).val(picker.startDate.format('YYYY-MM-DD'));
        reflashEvent("txtEventDate1","1");
       });
 
       $("#txtEventDate2").on('apply.daterangepicker', function(ev, picker) { 
        $(this).val(picker.startDate.format('YYYY-MM-DD'));
        reflashEvent("txtEventDate2","2");
       });
 
       $("#txtEventDate3").on('apply.daterangepicker', function(ev, picker) { 
        $(this).val(picker.startDate.format('YYYY-MM-DD'));
        reflashEvent("txtEventDate3","3");
       });

       $(".event").attr('disabled','disabled');
 }
     
}


//刷新event區塊
function reflashEvent(e,number) {

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Booking/getEvent/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ day: $("#"+e).val() ,guid:$("#hdnGuid").val()}),
        dataType: "text",
        cache: false,
        async: true,
        //timeout: 60000,
        error: function(jqXHR, textStatus, errorThrown) {

        },
        success: function(result) {

          var rsStatus =JSON.parse(result);
            if(rsStatus.status=="OK")
            {
              var options = "";
              options += '<option value=null disabled selected="selected">--</option>'; 

              for(i =0 ;i <rsStatus.dayevent.length;i++)
              {
                var id =rsStatus.dayevent[i].split('_')[0];
                var times=rsStatus.dayevent[i].split('_')[1];
                options += '<option value="' + id + '">' + times + '</option>';
              }
              $("#selEvent"+number).empty().append(options);
              $("#selEvent"+number).removeAttr('disabled');
            }
            else
            {
                alert(rsStatus.msgErr);
            }
        },
        complete: function() {

        }
    });
}



function setdataModel()
{
    //cusData
    var cusIdx =1;
    cusModel.forEach(function(e)
    {
        if($("#txtEngFirst_"+cusIdx).val()!="" &&  $("#txtEngFirst_"+cusIdx).is(":visible")==true)
            e.englishName.firstName =$("#txtEngFirst_"+cusIdx).val();
        if($("#txtEngLast_"+cusIdx).val()!="" &&  $("#txtEngLast_"+cusIdx).is(":visible")==true)
            e.englishName.lastName =$("#txtEngLast_"+cusIdx).val();
        if($("#selGender_"+cusIdx).val()!="" &&  $("#selGender_"+cusIdx).is(":visible")==true)
            e.gender =$("#selGender_"+cusIdx).val();
        if($("#txtBirtyday_"+cusIdx).val()!="" &&  $("#txtBirtyday_"+cusIdx).is(":visible")==true)
            e.birthday =$("#txtBirtyday_"+cusIdx).val();
        if($("#selcusCountry_"+cusIdx).val()!="" &&  $("#selcusCountry_"+cusIdx).is(":visible")==true)
            e.nationality.nationalityCode =$("#selcusCountry_"+cusIdx).val();
        if($("#txtPassportId_"+cusIdx).val()!="" &&  $("#txtPassportId_"+cusIdx).is(":visible")==true)
            e.passport.passportNo =$("#txtPassportId_"+cusIdx).val();        
        if($("#txtPassDate_"+cusIdx).val()!="" &&  $("#txtPassDate_"+cusIdx).is(":visible")==true)
            e.passport.passportExpDate =$("#txtPassDate_"+cusIdx).val();
        if($("#txtlocalFirstName_"+cusIdx).val()!="" &&  $("#txtlocalFirstName_"+cusIdx).is(":visible")==true)
            e.localName.firstName =$("#txtlocalFirstName_"+cusIdx).val();
        if($("#txtlocalLastName_"+cusIdx).val()!="" &&  $("#txtlocalLastName_"+cusIdx).is(":visible")==true)
            e.localName.lastName =$("#txtlocalLastName_"+cusIdx).val();
        if($("#txtNationTW_"+cusIdx).val()!="" &&  $("#txtNationTW_"+cusIdx).is(":visible")==true)
            e.nationality.TWIdentityNumber =$("#txtNationTW_"+cusIdx).val();       
        if($("#txtNationHKMO_"+cusIdx).val()!="" &&  $("#txtNationHKMO_"+cusIdx).is(":visible")==true)
            e.nationality.HKMOIdentityNumber =$("#txtNationHKMO_"+cusIdx).val();
        if($("#txtNationMTP_"+cusIdx).val()!="" &&  $("#txtNationMTP_"+cusIdx).is(":visible")==true)
            e.nationality.MTPNumber =$("#txtNationMTP_"+cusIdx).val();
        if($("#txtHigh_"+cusIdx).val()!="" &&  $("#txtHigh_"+cusIdx).is(":visible")==true)
            e.height.value =$("#txtHigh_"+cusIdx).val();
        if($("#selHighUnit_"+cusIdx).val()!="" &&  $("#selHighUnit_"+cusIdx).is(":visible")==true)
            e.height.unit =$("#selHighUnit_"+cusIdx).val();        
        if($("#txtWeight_"+cusIdx).val()!="" &&  $("#txtWeight_"+cusIdx).is(":visible")==true)
            e.weight.value=$("#txtWeight_"+cusIdx).val();
        if($("#selWeightUnit_"+cusIdx).val()!="" &&  $("#selWeightUnit_"+cusIdx).is(":visible")==true)
            e.weight.unit =$("#selWeightUnit_"+cusIdx).val();
        if($("#selSize_"+cusIdx).val()!="" &&  $("#selSize_"+cusIdx).is(":visible")==true)
            e.shoeSize.type =$("#selSize_"+cusIdx).val();
        if($("#selShoeUnitM_"+cusIdx).val()!="" &&  $("#selShoeUnitM_"+cusIdx).is(":visible")==true)
            e.shoeSize.unit =$("#selShoeUnitM_"+cusIdx).val().split('_')[0];
        if($("#selShoeUnitW_"+cusIdx).val()!="" &&  $("#selShoeUnitW_"+cusIdx).is(":visible")==true)
            e.shoeSize.unit =$("#selShoeUnitW_"+cusIdx).val().split('_')[0];
        if($("#selShoeUnitC_"+cusIdx).val()!="" &&  $("#selShoeUnitC_"+cusIdx).is(":visible")==true)
            e.shoeSize.unit =$("#selShoeUnitC_"+cusIdx).val().split('_')[0];
        if($("#selShoeSizeM_"+cusIdx).val()!="" &&  $("#selShoeSizeM_"+cusIdx).is(":visible")==true)
            e.shoeSize.value =$("#selShoeSizeM_"+cusIdx).val();
        if($("#selShoeSizeW_"+cusIdx).val()!="" &&  $("#selShoeSizeW_"+cusIdx).is(":visible")==true)
            e.shoeSize.value =$("#selShoeSizeW_"+cusIdx).val();
        if($("#selShoeSizeC_"+cusIdx).val()!="" &&  $("#selShoeSizeC_"+cusIdx).is(":visible")==true)
            e.shoeSize.value =$("#selShoeSizeC_"+cusIdx).val();
        if($("#selGlass_"+cusIdx).val()!="" &&  $("#selGlass_"+cusIdx).is(":visible")==true)
            e.glassDiopter =$("#selGlass_"+cusIdx).val();
        if($("#selMealType_"+cusIdx).val()!="" &&  $("#selMealType_"+cusIdx).is(":visible")==true)
            e.meal.mealType =$("#selMealType_"+cusIdx).val();
        
    });

    dataModel.travelerData =cusModel;

   //other
    if( $("#txtOtherModleNo").val()!="" && $("#txtOtherModleNo").is(":visible")==true)
        dataModel.modules.otherData.moduleData.mobileModelNumber =$("#txtOtherModleNo").val();
    if( $("#txtOtherImei").val()!="" && $("#txtOtherImei").is(":visible")==true)
        dataModel.modules.otherData.moduleData.mobileIMEI = $("#txtOtherImei").val();
    if( $("#txtOtherActDate").val()!="" && $("#txtOtherActDate").is(":visible")==true) 
        dataModel.modules.otherData.moduleData.activationDate =$("#txtOtherActDate").val();
    if( $("#selOtherLocation").val()!="" && $("#selOtherLocation").is(":visible")==true) 
        dataModel.modules.otherData.moduleData.exchangeLocationID =$("#selOtherLocation").val();

    //carPsgr
    if( $("#selCarPsgrAdtQty").val()!="" && $("#selCarPsgrAdtQty").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyAdult =$("#selCarPsgrAdtQty").val();
    if( $("#selCarPsgrChdQty").val()!="" && $("#selCarPsgrChdQty").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyChild =$("#selCarPsgrChdQty").val();
    if( $("#selCarPsgrInfQty").val()!="" && $("#selCarPsgrInfQty").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyInfant =$("#selCarPsgrInfQty").val();
    if( $("#selCarPsgrCarryLuggage").val()!="" && $("#selCarPsgrCarryLuggage").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyCarryLuggage =$("#selCarPsgrCarryLuggage").val();
    if( $("#selCarPsgrCheckedLuggage").val()!="" && $("#selCarPsgrCheckedLuggage").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyCheckedLuggage =$("#selCarPsgrCheckedLuggage").val();
    if( $("#selCarPsgrChdSupProvide").val()!="" && $("#selCarPsgrChdSupProvide").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyChildSeat.supplierProvided =$("#selCarPsgrChdSupProvide").val();
    if( $("#selCarPsgrChdSelfProvide").val()!="" && $("#selCarPsgrChdSelfProvide").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyChildSeat.selfProvided =$("#selCarPsgrChdSelfProvide").val();
    if( $("#selCarPsgrInfSupProvide").val()!="" && $("#selCarPsgrInfSupProvide").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyInfantSeat.supplierProvided =$("#selCarPsgrInfSupProvide").val();
    if( $("#selCarPsgrInfSelfProvide").val()!="" && $("#selCarPsgrInfSelfProvide").is(":visible")==true)
        dataModel.modules.passengerData.moduleData.qtyInfantSeat.selfProvided =$("#selCarPsgrInfSelfProvide").val();

    //send
    if( $("#txtSendDataRcefName").val()!="" && $("#txtSendDataRcefName").is(":visible")==true)
        dataModel.modules.sendData.moduleData.receiverName.firstName =$("#txtSendDataRcefName").val();
    if( $("#txtSendDataRcelName").val()!="" && $("#txtSendDataRcelName").is(":visible")==true)
        dataModel.modules.sendData.moduleData.receiverName.lastName =$("#txtSendDataRcelName").val();
    if( $("#selSendDataCountryCode").val()!="" && $("#selSendDataCountryCode").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToCountry.receiveAddress.countryCode =$("#selSendDataCountryCode").val();
    if( $("#selSendDataCityCode").val()!="" && $("#selSendDataCityCode").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToCountry.receiveAddress.cityCode =$("#selSendDataCityCode").val();
    if( $("#txtSendDataZipCode").val()!="" && $("#txtSendDataZipCode").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToCountry.receiveAddress.zipCode =$("#txtSendDataZipCode").val();
    if( $("#txtSendDataReceiverTel").val()!="" && $("#txtSendDataReceiverTel").is(":visible")==true)
        dataModel.modules.sendData.moduleData.receiverTel =$("#txtSendDataReceiverTel").val();
    if( $("#txtSendDataHtlName").val()!="" && $("#txtSendDataHtlName").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.hotelName =$("#txtSendDataHtlName").val();
    if( $("#txtSendDataHtlTel").val()!="" && $("#txtSendDataHtlTel").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.hotelTel =$("#txtSendDataHtlTel").val();
    if( $("#txtSendDataHtlAddress").val()!="" && $("#txtSendDataHtlAddress").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.hotelAddress =$("#txtSendDataHtlAddress").val();
    if( $("#txtSendDataBuyerPassfName").val()!="" && $("#txtSendDataBuyerPassfName").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.buyerPassportEnglishName.firstName =$("#txtSendDataBuyerPassfName").val();
    if( $("#txtSendDataBuyerPasslName").val()!="" && $("#txtSendDataBuyerPasslName").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.buyerPassportEnglishName.lastName =$("#txtSendDataBuyerPasslName").val();
    if( $("#txtSendDataBuyerLocalfName").val()!="" && $("#txtSendDataBuyerLocalfName").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.buyerLocalName.firstName =$("#txtSendDataBuyerLocalfName").val();
    if( $("#txtSendDatBuyerLocallName").val()!="" && $("#txtSendDatBuyerLocallName").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.buyerLocalName.lastName =$("#txtSendDatBuyerLocallName").val();
    if( $("#txtSendDataBookingWebSite").val()!="" && $("#txtSendDataBookingWebSite").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.bookingWebsite =$("#txtSendDataBookingWebSite").val();
    if( $("#txtSendDataBookingOrdNo").val()!="" && $("#txtSendDataBookingOrdNo").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.bookingOrderNo =$("#txtSendDataBookingOrdNo").val();
    if( $("#txtSendDataChkinDate").val()!="" && $("#txtSendDataChkinDate").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.checkInDate =$("#txtSendDataChkinDate").val();
    if( $("#txtSendDataChkoutDate").val()!="" && $("#txtSendDataChkoutDate").is(":visible")==true)
        dataModel.modules.sendData.moduleData.sendToHotel.checkOutDate =$("#txtSendDataChkoutDate").val();

    //contact
    if( $("#txtContactfName").val()!="" && $("#txtContactfName").is(":visible")==true)
        dataModel.modules.contactData.moduleData.contactName.firstName =$("#txtContactfName").val();
    if( $("#txtContactlName").val()!="" && $("#txtContactlName").is(":visible")==true)
        dataModel.modules.contactData.moduleData.contactName.lastName =$("#txtContactlName").val();
    if( $("#rdoContactPhone1").is(":visible")==true )
        dataModel.modules.contactData.moduleData.contactTel.haveTel =$('input[name*=contact_phone]:checked').val();
    if( $("#contact_phone").val()!="" && $("#contact_phone").is(":visible")==true && $('input[name*=contact_phone]:checked').val()=="true")
        dataModel.modules.contactData.moduleData.contactName.lastName =$("#contact_phone").val();
    if( $("#rdoContactApp1").is(":visible")==true )dataModel.modules.contactData.moduleData.contactApp.haveApp =$('input[name*=contact_app]:checked').val();
    if( $("#selContactApp").val()!="" && $("#selContactApp").is(":visible")==true && $('input[name*=contact_app]:checked').val()=="true")
        dataModel.modules.contactData.moduleData.contactApp.appType =$("#selContactApp").val();
    if( $("#txtContactAppAccount").val()!="" && $("#txtContactAppAccount").is(":visible")==true && $('input[name*=contact_app]:checked').val()=="true")
        dataModel.modules.contactData.moduleData.contactApp.appAccount  =$("#txtContactAppAccount").val();

    //guide
    if($("#selGuide").val()!="" &&  $("#selGuide").is(":visible")==true)dataModel.guideLang  =$("#selGuide").val();

    //rentCar
    if($("#selRentCarPickupOfiice").val()!="" &&  $("#selRentCarPickupOfiice").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.pickUp.officeID  =$("#selRentCarPickupOfiice").val();
    if($("#txtRendCarPickUpDate").val()!="" &&  $("#txtRendCarPickUpDate").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.pickUp.datetime.date  =$("#txtRendCarPickUpDate").val();
    if($("#selRentCarPickUpHour").val()!="" &&  $("#selRentCarPickUpHour").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.pickUp.datetime.hour  =$("#selRentCarPickUpHour").val();
    if($("#selRentCarPickUpMinute").val()!="" &&  $("#selRentCarPickUpMinute").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.pickUp.datetime.minute  =$("#selRentCarPickUpMinute").val();
    if($("#rdoWifiTrue").is(":visible")==true )dataModel.modules.carRentingData.moduleData.isNeedFreeWiFi =$('input[name*=rentCarwifi]:checked').val();
    if($("#rdoGpsTrue").is(":visible")==true )dataModel.modules.carRentingData.moduleData.isNeedFreeGPS =$('input[name*=rentCargps]:checked').val();
    if($("#selRentCarDropOffOfiice").val()!="" &&  $("#selRentCarDropOffOfiice").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.dropOff.officeID  =$("#selRentCarDropOffOfiice").val();
    if($("#txtRendCarPickUpDate").val()!="" &&  $("#txtRendCarPickUpDate").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.dropOff.datetime.date  =$("#txtRendCarPickUpDate").val();
    if($("#selRentCarPickUpHour").val()!="" &&  $("#selRentCarPickUpHour").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.dropOff.datetime.hour  =$("#selRentCarPickUpHour").val();
    if($("#selRentCarPickUpMinute").val()!="" &&  $("#selRentCarPickUpMinute").is(":visible")==true)
        dataModel.modules.carRentingData.moduleData.dropOff.datetime.minute  =$("#selRentCarPickUpMinute").val();
    dataModel.modules.carRentingData.moduleData.pickUp.datetime.date =$("#hdnUseDate").val();

    //shuttle
    if($("#txtShttleDate").val()!="" &&  $("#txtShttleDate").is(":visible")==true){ 
        dataModel.modules.shuttleData.moduleData.shuttleDate  =$("#txtShttleDate").val();}
    else {dataModel.modules.shuttleData.moduleData.shuttleDate  =$("#hdnUseDate").val();}
    
    if($("#selShuttlePickUpTime").val()=="customize" &&  $("#selShuttlePickUpTime").is(":visible")==true){
        dataModel.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.isCustom =true;} 
    else if($("#selShuttlePickUpTime").val()!="customize" &&  $("#selShuttlePickUpTime").is(":visible")==true) {
        dataModel.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.isCustom=false;}
    if($("#selShuttlePickUpTime").val()!="" && $("#selShuttlePickUpTime").val()!="customize" &&  $("#selShuttlePickUpTime").is(":visible")==true)
        dataModel.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.timeID  =$("#selShuttlePickUpTime").val();
    if($("#selShuttleCusHour").val()!="" &&  $("#selShuttleCusHour").is(":visible")==true)
        dataModel.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.hour  =$("#selShuttleCusHour").val();
    if($("#selShuttleCusMinute").val()!="" &&  $("#selShuttleCusMinute").is(":visible")==true)
        dataModel.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.minute  =$("#selShuttleCusMinute").val();
    if($("#txtShuttlePickUpLocation").val()!="" &&  $("#txtShuttlePickUpLocation").is(":visible")==true)
        dataModel.modules.shuttleData.moduleData.designatedByCustomer.pickUp.location  =$("#txtShuttlePickUpLocation").val();
    if($("#txtShuttleDropOffLocation").val()!="" &&  $("#txtShuttleDropOffLocation").is(":visible")==true)
        dataModel.modules.shuttleData.moduleData.designatedByCustomer.dropOff.location =$("#txtShuttleDropOffLocation").val();
    if($("#selShuttleLocationId").val()!="" &&  $("#selShuttleLocationId").is(":visible")==true)
        dataModel.modules.shuttleData.moduleData.designatedLocation.locationID  =$("#selShuttleLocationId").val();

    if($("#selShuttleCharterRoute").val()=="customize" &&  $("#selShuttleCharterRoute").is(":visible")==true){
        dataModel.modules.shuttleData.moduleData.charterRoute.isCustom =true;} 
    else if($("#selShuttleCharterRoute").val()!="customize" &&  $("#selShuttleCharterRoute").is(":visible")==true){
        dataModel.modules.shuttleData.moduleData.charterRoute.isCustom =false;} 
    if($("#selShuttleCharterRoute").val()!="" &&  $("#selShuttleCharterRoute").val()!="customize" &&  $("#selShuttleCharterRoute").is(":visible")==true){dataModel.modules.shuttleData.moduleData.charterRoute.routesID =$("#selShuttleCharterRoute").val();} 

    dataModel.modules.carRentingData.moduleData.pickUp.datetime.date =$("#hdnUseDate").val();

    //select-list
    if($("#selShuttleCharterRoute").val()=="customize" &&  $("#selShuttleCharterRoute").is(":visible")==true)
    {
        var customRoutes = [];
        $(".routeSelf").each(function()
        {
               customRoutes.push($(this).text());
        });
        dataModel.modules.shuttleData.moduleData.charterRoute.customRoutes=customRoutes;
    }

    //flight
    if($("#selArrFlightType").val()!="" &&  $("#selArrFlightType").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.flightType =$("#selArrFlightType").val();
    if($("#selArrAirport").val()!="" &&  $("#selArrAirport").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.airport  =$("#selArrAirport").val();
    if($("#txtArrTerminalNo").val()!="" &&  $("#txtArrTerminalNo").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.terminalNo  =$("#txtArrTerminalNo").val();
    if($("#txtArrAirline").val()!="" &&  $("#txtArrAirline").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.airline  =$("#txtArrAirline").val();
    if($("#txtArrFlightNo").val()!="" &&  $("#txtArrFlightNo").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.flightNo  =$("#txtArrFlightNo").val();
    if($("#txtArrDate").val()!="" &&  $("#txtArrDate").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.arrivalDatetime.date  =$("#txtArrDate").val();
    if($("#selArrHour").val()!="" &&  $("#selArrHour").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.arrivalDatetime.hour  =$("#selArrHour").val();
    if($("#selArrMinute").val()!="" &&  $("#selArrMinute").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.arrivalDatetime.minute  =$("#selArrMinute").val();
    if($("#VisaType").val()!="" &&  $("#rdVisaTypeTrue").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.arrival.isNeedToApplyVisa  =$('input[name*=VisaType]:checked').val();
    if($("#selDepFlightType").val()!="" &&  $("#selDepFlightType").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.departure.flightType =$("#selDepFlightType").val();
    if($("#selDepAirport").val()!="" &&  $("#selDepAirport").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.departure.flightType  =$("#selDepAirport").val();
    if($("#txtDepTerminalNo").val()!="" &&  $("#txtDepTerminalNo").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.departure.terminalNo =$("#txtDepTerminalNo").val();
    if($("#txtDepAirline").val()!="" &&  $("#txtDepAirline").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.departure.airline  =$("#txtDepAirline").val();
    if($("#txtDepDate").val()!="" &&  $("#txtDepDate").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.departure.departureDatetime.date  =$("#txtDepDate").val();
    if($("#selDepHour").val()!="" &&  $("#selDepHour").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.departure.departureDatetime.hour  =$("#selDepHour").val();
    if($("#selDepMinute").val()!="" &&  $("#selDepMinute").is(":visible")==true)
        dataModel.modules.flightInfoData.moduleData.departure.departureDatetime.minute  =$("#selDepMinute").val();
    dataModel.modules.flightInfoData.moduleData.departure.haveBeenInCountry=false;

    //eventBackup
    var eventBackup="";
    if( $("#txtEventDate1").val()!="" &&  $("#selEvent1").val()!="" 
        && $("#selEvent1").is(":visible")==true && $("#selEvent1").is(":disabled")==false)
    {
        eventBackup ="1/"+$("#txtEventDate1").val()+"/"+ $("#selEvent1").val()
        if( $("#txtEventDate2").val()!="" &&  $("#selEvent2").val()!="" 
            && $("#selEvent2").is(":visible")==true &&  $("#selEvent2").is(":disabled")==false)
        {
            eventBackup =",2/"+$("#txtEventDate2").val()+"/"+ $("#selEvent2").val()+","
            if( $("#txtEventDate3").val()!="" &&  $("#selEvent3").val()!="" 
            && $("#selEvent3").is(":visible")==true &&  $("#selEvent3").is(":disabled")==false)
            {
              eventBackup =",3/"+$("#txtEventDate3").val()+"/"+ $("#selEvent3").val()
            }
        }
        dataModel.eventBackupData =eventBackup;
    }

    //pay
    if($("#txtPayHolderName").val()!="" &&  $("#txtPayHolderName").is(":visible")==true)
        dataModel.card.cardHolder  =$("#txtPayHolderName").val();
    if($("#txtPayCardNum").val()!="" &&  $("#txtPayCardNum").is(":visible")==true)
        dataModel.card.cardNo  =$("#txtPayCardNum").val().replace(" ","").replace(" ","").replace(" ","");
    if($("#txtPayExpireDate").val()!="" &&  $("#txtPayExpireDate").is(":visible")==true)
        dataModel.card.expiry  =$("#txtPayExpireDate").val().replace("/","");
    if($("#txtPayCvc").val()!="" &&  $("#txtPayCvc").is(":visible")==true)
        dataModel.card.cardCvv  =$("#txtPayCvc").val();

    dataModel.note = $("#txtOrdNote").val();

} 



//測試用
function setModule()
{
}


//contactEvent
function showContatPhone(e)
{
   if(e=="1")
   {
     $("#txtContactPhone").show();
   }
   else
   {
     $("#txtContactPhone").hide();
   }
}

function showContactApp(e)
{
   if(e=="1")
   {
     $("#selContactApp").show();
     $("#txtContactAppAccount").show();
   }
   else
   {
     $("#selContactApp").hide();
     $("#txtContactAppAccount").hide();
   }
}

function formatDate(inDate)
{
    if(inDate.length==8)
    {
        return inDate.substr(0, 4)+"-"+inDate.substr(4, 2) +"-"+inDate.substr(6, 2);
    }
    else 
    {
         return inDate;
    }
}

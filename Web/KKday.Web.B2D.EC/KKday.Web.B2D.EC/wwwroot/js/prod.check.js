
function setInit()
{
    $(".confirmBar").hide();
    $(".minPriceBar").show();

    if($("#hdnIsEcSale").val()=="True")reflashPkg();

}

 

function setDatePicker(e)
{
    var cutOfDay ='+'+$("#hdnCutOfDay").val()+'d';
   
    $('#datepicker1').datepicker({ 
                autoclose:true,
                format: 'yyyymmdd',
                startDate: cutOfDay,

                beforeShowDay : function(d)
                {
                    var allCanUseDate ="";
                    if(e=="1")
                    {
                       allCanUseDate =$("#hdnAllCanUseDate").val();
                    }
                    else 
                    {
                       if($("#hdnPkgOid").val()!="")
                       {
                         allCanUseDate =$("#hdnPkgDate_"+$("#hdnPkgOid").val()).val();
                       }
                       else 
                       {
                         allCanUseDate =$("#hdnAllCanUseDate").val();
                       }
                    }
                    var allCanUseDateArr =allCanUseDate.split(',');
                    
                    var IsPermit =true;
                    //allCanUseDateArr.forEach(function(element) {

                    var day= d.getDay(); //
                    var day = d.getDate().toString();
                    if(day <10) day ='0'+day;
                    var month = (d.getMonth()+1).toString();
                    if (month<10) month='0'+month;
                    var year = (d.getFullYear()).toString();
                    var daytemp =year+month+day;

                    //console.log(daytemp);
 
                     var chk = allCanUseDateArr.find(function(item, index, array){
                     return item ==daytemp;           
                     });

                     if (chk==daytemp)
                     {
                       IsPermit =true;
                     }
                     else
                     {
                       IsPermit =false;
                     }

                 //}
                    return IsPermit;
                }
     }).on('changeDate', function(e) {
          // `e` here contains the extra attributes
          //if($("#hdnPreSelDate").val()!=$("#selDate").val())
          //{
             //$("#datepicker1").datepicker('hide');
             //$("#hdnChkflow").val("1");
             if($("#selDate").val()!="") $("#hdnPreSelDate").val($("#selDate").val());
             reflashPkg(); //重新找可用套餐
             console.log("3");
          // }

    }).on('clearDate',function(e){
        if($("#hdnPreSelDate").val()!="") $("#selDate").val($("#hdnPreSelDate").val());
    });
}

function iniShowDate()
{
   //選擇其他日期以套餐的日期為主
   $('#datepicker1').datepicker('destroy');
   setDatePicker('1');
   if($("#selDate").val()!="")
   {
      var sd =formatDate($("#selDate").val());
      $("#datepicker1").datepicker('setDate',sd);
   }
   $("#datepicker1").datepicker('show');
}

function chgPkgInfo(pkgOid,chk)
{
   //var thisDate=$("#selDate").val();

   if(pkgOid =='')
   {
     //$("#hdnPkgOid").val("");
     $('#datepicker1').datepicker('destroy');
     setDatePicker('2');

     if($("#selDate").val()!="")
     {
        var sd =formatDate($("#selDate").val());
        $("#datepicker1").datepicker('setDate',sd);
     }
     $("#datepicker1").datepicker('show');
   }
   else 
   {
       $("#hdnPkgOid").val(pkgOid);
       if(chk=='showprice'  && $("#selDate").val()!="")
       {
           //歸0
          $("input[id^='txtprice']").val("0");
          //把選擇數量秀出來 
          $("#divPrice_"+pkgOid).css("display","inline");
          $("input[id^='selminus']").addClass("disabledClass");
          $("input[id^='selplus']").removeClass("disabledClass");

          $(".confirmBar").show();
          $(".minPriceBar").hide();
          $("#confirmName").text($("#hdnPkgName_"+pkgOid).val());
          $("#confirmSelDate").text($("#selDate").val());
          $("#confirmPrice").text("0");
          console.log("2");
       }
       else //showPkgDate
       { 
           //選擇其他日期以套餐的日期為主
           $('#datepicker1').datepicker('destroy');
           setDatePicker('2');

           if($("#selDate").val()!="")
           {
              var sd =formatDate($("#selDate").val());
              $("#datepicker1").datepicker('setDate',sd);
           }
           $("#datepicker1").datepicker('show');

           if(chk =="first"){ $("#hdnShowImmediately").val("1");}

       }
   }

}


//刷新套餐區塊
function reflashPkg() {

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Product/reflashPkg/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ prodOid: $("#hdnProdOid").val(),selDate :$("#selDate").val() }),
        dataType: "text",
        cache: false,
        async: true,
        //timeout: 60000,
        error: function (jqXHR, textStatus, errorThrown) {
            alert('要改');
        },
        success: function (result) {
            $("#showPkg").html(result);
            if($("#hdnPkgOid").val()!="" && $("#hdnShowImmediately").val()=="1")
            {
               console.log("1");
               chgPkgInfo($("#hdnPkgOid").val(),"showprice");
               $("hdnShowImmediately").val("");
            }
        },
        complete: function () {
             

 
        }
    });
}

//選擇數量及金額計算
function chkNum(pkgOid,qtyId,cond,priceType)
{ 
  var qty =$("#hdnOrderQty_"+pkgOid).val();
  var qtyArr =qty.split(',');
  var nowQty = $("#"+qtyId).val();
  
  var qtyindex =qtyArr.findIndex(function(item, index, array){
                     return item ==nowQty;           
                     });
  if(nowQty=="0" && cond=="plus" )
  {
    $("#"+qtyId).val(qtyArr[0]);
  }
  else if (nowQty=="1" && cond=="minus" )
  {
    $("#"+qtyId).val("0");
  }
  else 
  {
    if(cond=="plus")
    {
      $("#"+qtyId).val(qtyArr[parseInt(qtyindex)+1]);
      //$("#selminus_"+priceType+"_"+pkgOid).removeClass("disabledClass");
    }
    else 
    {
      $("#"+qtyId).val(qtyArr[parseInt(qtyindex)-1]);
      //$("#selplus_"+priceType+"_"+pkgOid).removeClass("disabledClass");
    }
  }

  if(cond=="plus")
  {
   $("#selminus_"+priceType+"_"+pkgOid).removeClass("disabledClass");
  }
  else   
  { 
   $("#selplus_"+priceType+"_"+pkgOid).removeClass("disabledClass");
  }

  if($("#"+qtyId).val()=="0")
    { $("#selminus_"+priceType+"_"+pkgOid).addClass("disabledClass"); }
    
  if($("#"+qtyId).val()==qtyArr[qtyArr.length-1] )
    {$("#selplus_"+priceType+"_"+pkgOid).addClass("disabledClass");}

   confirmTotalPrice(pkgOid);
}


//confirm
function confirmTotalPrice(pkgOid)
{
   //數量
   var adtPrice=0;
   var chdPrice=0;
   var infPrice=0;
   var eldPrice=0;

   $("#hdnConfirmPkgOid").val(pkgOid);
   adtPrice =parseInt( $("#txtprice1Qty_"+pkgOid).val())* parseFloat($("#hdnPrice1_"+pkgOid).val());
   
   if($("#hdnPrice2_"+pkgOid).val()!=""){
      chdPrice =parseInt( $("#txtprice2Qty_"+pkgOid).val())* parseFloat($("#hdnPrice2_"+pkgOid).val())
   }

   if($("#hdnPrice3_"+pkgOid).val()!=""){
      infPrice =parseInt( $("#txtprice3Qty_"+pkgOid).val())* parseFloat($("#hdnPrice3_"+pkgOid).val())
   }  

   if($("#hdnPrice4_"+pkgOid).val()!=""){
      eldPrice =parseInt( $("#txtprice4Qty_"+pkgOid).val())* parseFloat($("#hdnPrice4_"+pkgOid).val())
   }

   var totalPrice =(adtPrice+chdPrice+infPrice+eldPrice);
   $("#confirmPrice").text(totalPrice);
}


function btnConfirm()
{
    var pkgOid = $("#hdnPkgOid").val();
    //確認數量有被選擇
    
    alert(pkgOid);
    
    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Product/confirmPkg/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ prodOid: $("#hdnProdOid").val(),selDate :$("#selDate").val(),
            pkgOid :$("#hdnPkgOid").val(),
            price1Qty :chkQty($("#txtprice1Qty_"+pkgOid).val()),price2Qty :chkQty($("#txtprice2Qty_"+pkgOid).val()),
            price3Qty:chkQty($("#txtprice3Qty_"+pkgOid).val()),price4Qty:chkQty($("#txtprice4Qty_"+pkgOid).val()),
            guid: $("#hdnGuid").val() }),
        dataType: "json",
        cache: false,
        async: true,
        //timeout: 60000,
        error: function (jqXHR, textStatus, errorThrown) {
            alert('要改');
        },
        success: function(result) {
           
            if(result.status=="OK"){
               // window.location.reload();
            } else{
                 alert(result.msg);
            }
        },
        complete: function () {
             //window.open();
             //window.location.replace(_root_path +"Booking/"+ $("#hdnGuid").val());
             PostToUrl(_root_path +"Booking/",{guid:$("#hdnGuid").val()},'',false);
        }
    });
}

function chkQty(e)
{

    if(e =="" ||(typeof(e) == "undefined")){return "0";}
    else {return e;}
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

function rtnYear(inDate)
{
  if(inDate.length==8)
    {
        return inDate.substr(0, 4);
    }
    else 
    {
         return inDate;
    }
}


function rtnMonth(inDate)
{
    if(inDate.length==8)
    {
        var m = parseInt(inDate.substr(4, 2))-1;
        return m;
    }
    else 
    {
         return inDate;
    }
}



//Javascript 發出 POST Request
function PostToUrl(path, params, method, newWindow) {
    method = method || "post"; //預設post
    newWindow = newWindow || false; //預設不另開

    //建立一個form
    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);
    if (newWindow) form.setAttribute("target", "_blank");

    //Form Value (用 hidden)
    for (var key in params) {
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", key);
        hiddenField.setAttribute("value", params[key]);

        form.appendChild(hiddenField);
    }

    //submit
    document.body.appendChild(form);
    form.submit();

    //Sample
    //PostToUrl("/Booking/FillData", { s: "aaa" });
}
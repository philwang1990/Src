
function initModule3()
{
    shuttleCustomTime('hide');
    shuttleCustomRoute('hide');
}


//select nation..
function chgNation(i)
{

   if($("#selcusCountry_"+i).val()=="TW" && $("#hdnNationTW_"+i).val()=="Y")
   {
     $(".nationTW_"+i).show();
     $(".nationHKMO_"+i).hide();
     $(".nationMTP_"+i).hide();
   }
   else if( ($("#selcusCountry_"+i).val()=="HK" || $("#selcusCountry_"+i).val()=="MO" )  &&  $("#hdnNationHKMO_"+i).val()=="Y")
   {
     $(".nationTW_"+i).hide();
     $(".nationHKMO_"+i).show();
     $(".nationMTP_"+i).hide();
   }
   else if( $("#selcusCountry_"+i).val()=="CN"   &&  $("#hdnNationMTP_"+i).val()=="Y")
   {
     $(".nationTW_"+i).hide();
     $(".nationHKMO_"+i).hide();
     $(".nationMTP_"+i).show();
   }
   else
   {
     $(".nationTW_"+i).hide();
     $(".nationHKMO_"+i).hide();
     $(".nationMTP_"+i).hide();
   }
}

function chgShuttleRoute(e)
{
  var id =$(e).val();

   if (id=="customize")
   {
      shuttleCustomRoute('show');
   }
   else
   {
      shuttleCustomRoute('hide');
   }
}

function shuttleCustomRoute(e)
{
   if (e=="show")
   {
     $(".shuttleCusRoute").show();
   }
   else
   {
     $(".shuttleCusRoute").hide();
   }
}

//select shuttlePickUpTime()
function chgShuttlePickupTime(e)
{
   var id =$(e).val();

   if (id=="customize")
   {
      shuttleCustomTime('show');
   }
   else
   {
      shuttleCustomTime('hide');
   }
}

//select shuttleLocation
function chgShuttleLocation(e)
{
    var id =$(e).val();
    if(id!="")
    {
        var add =$("#hdnLocation_"+id).val();
        $(".venueLocationTip").html( add);
    }
}


function shuttleCustomTime(e)
{
   if (e=="show")
   {
     $(".shuttleCustomTime").show();
     setShuttleCustomHour();
   }
   else
   {
     $(".shuttleCustomTime").hide();
   }
}


//select shuttleCusHour
function  setShuttleCustomHour()
{
    var info=$("#hdnShuttle_CustomTime").val();
    if(info!="")
    {
        
         var sh =info.split('%')[0].split(':')[0]; 
         var eh =info.split('%')[1].split(':')[0]; 

         var options = "";
         options += '<option value=null disabled selected="selected">--</option>'; 

         for( ii=parseInt(sh) ;ii<=parseInt(eh) ;ii++)
         {
           var iii =ii
           if( iii<10)  iii="0"+iii;
           options += '<option value="' + iii + '">' + iii + '</option>';
         }
         $("select[id^=selShuttleCusHour]").empty().append(options);
         $("select[id^=selShuttleCusHour]").val("--");

    }
}

//select shuttleCusHur
function chgCustomHour(e)
{
   var info = $("#hdnShuttle_CustomTime").val();
    if (info!="" && e !=null)
    {
         var sh =info.split('%')[0].split(':')[0]; if( parseInt(sh)<10 )sh="0"+sh;
         var eh =info.split('%')[1].split(':')[0]; if( parseInt(eh)<10 )eh="0"+eh;
         var st =info.split('%')[0].split(':')[1];
         var et =info.split('%')[1].split(':')[1];

         var options = "";
         options += '<option value=null disabled selected="selected">--</option>'; 
         if( $(e).val()==sh)
         {
           for( ii=parseInt(st) ;ii<=55 ;ii=ii+5)
           {
              var iii =ii
              if( ii<10)  iii="0"+iii;
              options += '<option value="' + iii + '">' + iii + '</option>';
           }
         }
         else if ($(e).val()==eh)
         {
           for( ii=0 ;ii<=parseInt(et) ;ii=ii+5)
           {
              var iii =ii
              if( ii<10)  iii="0"+iii;
              options += '<option value="' + iii + '">' + iii + '</option>';
           }
         }
         else
         {
           for( ii=0 ;ii<=55 ;ii=ii+5)
           {
              var iii =ii
              if( iii<10)  iii="0"+iii;
              options += '<option value="' + iii + '">' + iii + '</option>';
           }
         }
      $("select[id^=selShuttleCusMinute]").empty().append(options);
      //$("select[id^=selShuttleCusMinute]").val("--");

    }
    else 
    {
      var options = "";
      options += '<option value=null disabled selected="selected">--</option>';
      $("select[id^=selRentCarPickUpMinute]").empty().append(options);
      //$("select[id^=selRentCarPickUpMinute]").val("--");
    }
}


//select rendCaroffice
function chgRentCarTip(s,e)
{
    if(s=='P')
    {
        var info = $("#hdnPickup_"+$(e).val()).val();
        if (info!="")
        {
            $(".pickupTip").html( info.split('%')[4]);
             var sh =info.split('%')[0];
             var eh =info.split('%')[2]; 
             var st =info.split('%')[1];
             var et =info.split('%')[3];

             var options = "";
             options += '<option value=null disabled selected="selected">--</option>'; 

             for( ii=parseInt(sh) ;ii<=parseInt(eh) ;ii++)
             {
               var iii =ii
               if( iii<10)  iii="0"+iii;
               options += '<option value="' + iii + '">' + iii + '</option>';
             }
             $("select[id^=selRentCarPickUpHour]").empty().append(options);
             $("select[id^=selRentCarPickUpHour]").val("--");
             $("#hdnSelPickUpId").val($(e).val());
        }
        else
        {
             var options = "";
             options += '<option value=null disabled selected="selected">--</option>'; 
             $("select[id^=selRentCarPickUpHour]").empty().append(options);
             $("select[id^=selRentCarPickUpHour]").val("--");
             $("#hdnSelPickUpId").val("");
        }
        chgRentCarHour("P",null);
    }
    else
    {
        var info = $("#hdnPickup_"+$(e).val()).val();
        if (info!="")
        {
            $(".dropOffTip").html( info.split('%')[4]);
             var sh =info.split('%')[0];
             var eh =info.split('%')[2]; 
             var st =info.split('%')[1];
             var et =info.split('%')[3];

             var options = "";
             options += '<option value=null disabled selected="selected">--</option>'; 

             for( ii=parseInt(sh) ;ii<=parseInt(eh) ;ii++)
             {
               var iii =ii
               if( iii<10)  iii="0"+iii;
               options += '<option value="' + iii + '">' + iii + '</option>';
             }
             $("select[id^=selRentCarDropOffHour]").empty().append(options);
             $("select[id^=selRentCarDropOffHour]").val("--");
             $("#hdnSelDropOffId").val($(e).val());
 
        }
        else
        {
             var options = "";
             options += '<option value=null disabled selected="selected">--</option>'; 
             $("select[id^=selRentCarDropOffHour]").empty().append(options);
             $("select[id^=selRentCarDropOffHour]").val("--");
             $("#hdnSelDropOffId").val("");
        }
        chgRentCarHour("D",null);
    }

}

function chgRentCarHour(s,e)
{
   if(s=="P")
   {
        var info = $("#hdnPickup_"+$("#hdnSelPickUpId").val()).val();
        if (info!="" && e !=null)
        {
             var sh =info.split('%')[0]; if( parseInt(sh)<10 )sh="0"+sh;
             var eh =info.split('%')[2]; if( parseInt(eh)<10 )eh="0"+eh;
             var st =info.split('%')[1];
             var et =info.split('%')[3];

             var options = "";
             options += '<option value=null disabled selected="selected">--</option>'; 
             if( $(e).val()==sh)
             {
               for( ii=parseInt(st) ;ii<=50 ;ii=ii+10)
               {
                  var iii =ii
                  if( ii==0)  iii="0"+iii;
                  options += '<option value="' + iii + '">' + iii + '</option>';
               }
             }
             else if ($(e).val()==eh)
             {
               for( ii=0 ;ii<=parseInt(et) ;ii=ii+10)
               {
                  var iii =ii
                  if( ii==0)  iii="0"+iii;
                  options += '<option value="' + iii + '">' + iii + '</option>';
               }
             }
             else
             {
               for( ii=0 ;ii<=50 ;ii=ii+10)
               {
                  var iii =ii
                  if( iii==0)  iii="0"+iii;
                  options += '<option value="' + iii + '">' + iii + '</option>';
               }
             }
          $("select[id^=selRentCarPickUpMinute]").empty().append(options);
         $("select[id^=selRentCarPickUpMinute]").val("--");
 
        }
        else 
        {
          var options = "";
          options += '<option value=null disabled selected="selected">--</option>';
          $("select[id^=selRentCarPickUpMinute]").empty().append(options);
          $("select[id^=selRentCarPickUpMinute]").val("--");
        }
   }
   else
   {
     var info = $("#hdnPickup_"+$("#hdnSelDropOffId").val()).val();
        if (info!="" && e !=null)
        {
             var sh =info.split('%')[0]; if( parseInt(sh)<10 )sh="0"+sh;
             var eh =info.split('%')[2]; if( parseInt(eh)<10 )eh="0"+eh;
             var st =info.split('%')[1];
             var et =info.split('%')[3];

             var options = "";
             options += '<option value=null disabled selected="selected">--</option>'; 
             if( $(e).val()==sh)
             {
               for( ii=parseInt(st) ;ii<=50 ;ii=ii+10)
               {
                  var iii =ii
                  if( ii==0)  iii="0"+iii;
                  options += '<option value="' + iii + '">' + iii + '</option>';
               }
             }
             else if ($(e).val()==eh)
             {
               for( ii=0 ;ii<=parseInt(et) ;ii=ii+10)
               {
                  var iii =ii
                  if( ii==0)  iii="0"+iii;
                  options += '<option value="' + iii + '">' + iii + '</option>';
               }
             }
             else
             {
               for( ii=0 ;ii<=50 ;ii=ii+10)
               {
                  var iii =ii
                  if( iii==0)  iii="0"+iii;
                  options += '<option value="' + iii + '">' + iii + '</option>';
               }
             }
          $("select[id^=selRentCarDropoffMinute]").empty().append(options);
          $("select[id^=selRentCarDropoffMinute]").val("--");
 
        }
        else 
        {
          var options = "";
          options += '<option value=null disabled selected="selected">--</option>';
          $("select[id^=selRentCarPickUpMinute]").empty().append(options);
          $("select[id^=selRentCarPickUpMinute]").val("--");
        }
   }
}


//select shoes
function chgShoeIdentity(i)
{
    if ($("#selSize_"+i).val().split('_')[0]=="M")
    {
        chgShoeIdentity2(i,true,false,false);
        $(".shoeSizeM_"+i).show();
    }
    else if ($("#selSize_"+i).val().split('_')[0]=="W")
    {
        chgShoeIdentity2(i,false,true,false);
        $(".shoeSizeW_"+i).show();
    }
    else if ($("#selSize_"+i).val().split('_')[0]=="C")
    {
        chgShoeIdentity2(i,false,false,true);
        $(".shoeSizeC_"+i).show();
    }
        else
    {
        chgShoeIdentity2(i,false,false,false);
       $(".shoeSizeM_"+i).hide();
       $(".shoeSizeW_"+i).hide();
       $(".shoeSizeC_"+i).hide();
    }
}

function chgShoeIdentity2(i,m,w,c)
{
    if(m==true)
    {
        $(".shoeUnitMan_"+i).show();
 
    }
    else 
    {
        $(".shoeUnitMan_"+i).hide();
 
    }
    if(w==true)
    {
        $(".shoeUnitWoman_"+i).show();
 
    }
    else 
    {
        $(".shoeUnitWoman_"+i).hide();
 
    }
    if(c==true)
    {
        $(".shoeUnitChild_"+i).show();
    }
    else 
    {
        $(".shoeUnitChild_"+i).hide();
    
    }
}

function chgShoeIdentity3(t)
{
    if($(t).val()!="")
    {
      var s = parseFloat($(t).val().split('_')[1]);
      var e = parseFloat($(t).val().split('_')[2]);
      var options = "";
      options += '<option value=null disabled selected="selected">--</option>'; 
      for (ii=s ; ii<= e ; ii=ii+0.5 )
      {
        options += '<option value="' + ii + '">' + ii + '</option>';
      }
    
      $("select[id^=selShoeSize]").empty().append(options);
      $("select[id^=selShoeSize]").val("--");
    }
    else
    {
      $("select[id^=selShoeSize]").empty();
    }

}

function addShuttleCusRoute()
{
   if ($("#txtShuttleCusRoute").val()!="")
   {
      var countLimit = parseInt( $("#hdnShuttleCusRouteCount").val()); 
      //var button = document.createElement("input");
      //button.type = "button";
      //button.id= $("#txtShuttleCusRoute").val()
      //button.value = $("#txtShuttleCusRoute").val();
      //button.className ="btn-default routeSelf";
      //button.onclick = function delShuttleRoute(this);
      //$(".shuttleCurRouteSelf").append(button);
      var li ="<li class='routeSelf' onclick='delShuttleRoute(this)'>"+$("#txtShuttleCusRoute").val()+"</li>"; 
       $(".select-list").append(li);

      var countTotal=0;
       $(".routeSelf").each(function(index)
       {
           countTotal++;
       });
        
       if (countLimit ==countTotal)
       {
           $("#btnShuttleCusRoute").attr("disabled","disabled");
       }
   }
   
}

function delShuttleRoute(e)
{
   $(e).remove();
   $("#btnShuttleCusRoute").attr("disabled",false);
}

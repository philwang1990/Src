/*
	kkday Jquery Plugin . DIY
*/



// datepackier for kkday
// 2014.08.22
// Name kkcalender
// For index use
// call back : function kkcalendarReturn()
// have a nice day
// ----------------------------------------------------------------------------------------------------------------------------------------
(function( $ ){

  jQuery.fn.kkcalendar = function(options) {
    var td    = new Date() ;
    var today = new Date( td.getFullYear(), td.getMonth(), td.getDate() ) ;
	var defaultOnDate = getDefaultOnDate();
    var monthName = [ 'Jan' , 'Feb' , 'Mar' , 'Apr' , 'May' , 'Jun' , 'Jul' , 'Aug' , 'Sep' , 'Oct' , 'Nov' , 'Dec'  ] ;
    var _defaultSettings = {
          today       :   today ,
          monthName   :   monthName ,
          on_date     :   defaultOnDate,
          startDate   :   '1/11/2000' ,
          endDate     :   '12/30/2020' ,
          newrule     :   '',
		  callback    :   null
    };
    var _settings = $.extend(_defaultSettings, options);

	function getDefaultOnDate(){
		var today = new Date();
		var onDt = {}
		for (var i = 0; i < 365; i++) {
			var day = new Date(today.getFullYear(), today.getMonth(), today.getDate() + i)
			var year = day.getFullYear();
			var month = day.getMonth() + 1;
			var date = day.getDate();

			if (year in onDt) {
				if (month in onDt[year]) {
					onDt[year][month][date] = 'Y';
				} else {
					onDt[year][month] = {};
					onDt[year][month][date] = 'Y';
				}
			} else {
				onDt[year] = {};
				onDt[year][month] = {};
				onDt[year][month][date] = 'Y';
			}
		}
		return JSON.stringify(onDt);
	}

    // makeCalender
    function makeCalendar( thisId , today , on_date , startDate , endDate  , newrule , callback)
    {
      // today.setDate(today.getDate() + 14);
      var year  = today.getFullYear() ;
      var month = today.getMonth() ;

      //set str innerHtml --------------------------------------------------------------------------------------------------------
      var str   = '' ;
      str += '<div class="row calander-title">' ;
      str += '  <div>' ;
      str += '    <a class=" col-xs-3 datepicker-prev" id="calender_left">' ;
      str += '      <span class="glyphicon glyphicon-chevron-left"></span>' ;
      str += '    </a>' ;
      str += '  </div>' ;
      str += '  <div class=" col-xs-6 h4 text-center">'+monthName[month]+' '+year+'</div>' ;
      str += '  <div>' ;
      str += '    <a class=" col-xs-3 datepicker-next" id="calender_right">' ;
      str += '      <span class="glyphicon glyphicon-chevron-right"></span>' ;
      str += '    </a>' ;
      str += '  </div>' ;
      str += '</div>' ;

      //set realtoday
      var realtodaytmp = new Date() ;
      realtodaytmp.setDate(realtodaytmp.getDate() + 14);
      var realtoday = new Date( realtodaytmp.getFullYear() , realtodaytmp.getMonth() , realtodaytmp.getDate() ) ;

      str += '<table border="1"><tr><th>Sun</th><th>Mon</th><th>Tue</th><th>Wed</th><th>Thu</th><th>Fri</th><th>Sat</th></tr>';
      var dt = new Date( year, month, 1);
      var wd = dt.getDay(); //week day
      var i;

      str += '<tr>';
      on_date_array = jQuery.parseJSON( on_date );
      var thismonth = parseInt(month)+1 ;
      for( ; wd > 0 ; wd--)   str+='<td>';
      for( i=1; i<32; )
      {
        dt = new Date(year, month, i);
        wd = dt.getDay();

        if ( on_date_array.hasOwnProperty( year ) )
        {
          if ( on_date_array[year].hasOwnProperty( thismonth ) )
          {
            if ( on_date_array[year][thismonth].hasOwnProperty( i ) )
            {
                  if( on_date_array[year][thismonth][i] == 'Y' )
                  {
                    if( (Date.parse(startDate).valueOf() <= Date.parse(dt).valueOf()) &&
                        (Date.parse(dt).valueOf() <= Date.parse(endDate).valueOf() ) )
                    {
                        str+='<td class="calandarTD">'+i;
                    }
                    else
                      str += '<td class="date-unselect">' + i ;

                  }
                  else
                    str += '<td class="date-unselect">' + i ;
            }
            else
              str += '<td class="date-unselect">' + i ;
          }
          else
            str += '<td class="date-unselect">' + i ;
        }
        else
          str += '<td class="date-unselect">' + i ;

        dt = new Date(year, month, ++i) ;
        if( dt.getMonth() != month ) break;
        wd = dt.getDay();
        if( wd == 0 )
          str += '<tr>';
      }
      for( ; wd < 6; wd++)  str+='<td>';
      str += '</table>';

      //output
      $( thisId ).html( str ) ;

      //set click
      $('#calender_left').click(function(event) {
        $('#calandar').kkcalendar(
                          {
                            'today'     :   new Date( today.getFullYear(), today.getMonth()-1, 1 ) ,
                            'on_date'   :   on_date ,
                            'startDate' :   startDate ,
                            'endDate'   :   endDate,
							'callback'	:	callback
                          }
                        );
      });
      $('#calender_right').click(function(event) {
        $('#calandar').kkcalendar(
                          {
                            'today'     :   new Date( today.getFullYear(), today.getMonth()+1, 1 ) ,
                            'on_date'   :   on_date ,
                            'startDate' :   startDate ,
                            'endDate'   :   endDate,
							'callback'	:	callback
                          }
                        );
      });

      $('.calandarTD').each(function(index, el) {
          $(this).click(function(event) {
             //console.log( (month+1)+'/'+$(this).html()+'/'+'/'+year ) ;
			 if(_settings.callback == null){
				 kkcalendarReturn( new Date( ((month<9)?'0':'')+(month+1)+'/'+((parseInt($(this).text())<10)?'0':'')+$(this).text()+'/'+year)) ;
			 }else {
			 	_settings.callback(new Date( ((month<9)?'0':'')+(month+1)+'/'+((parseInt($(this).text())<10)?'0':'')+$(this).text()+'/'+year));
			 }
          });
      });

    } ;
    //return HTML
    return this.each(function() {
      makeCalendar( this , _settings.today , _settings.on_date , _settings.startDate , _settings.endDate , _settings.newrule  , _settings.callback) ;
    });
  };

})(jQuery);
// ----------------------------------------------------------------------------------------------------------------------------------------





// product item for kkday
// Name product_item
// For product search use
// Object
// have a nice day
// ----------------------------------------------------------------------------------------------------------------------------------------
function ProductItem(sessionLang)
{

  var ccy = 'USD' ;
  this.setccy = function (v)
  {
    ccy = v;
  }

  // search Bar
  //
  this.ToolBarView = function( ViewCount , Country , City , sort )
  {
    //console.log( sort ) ;

    if( City != '')
      City = ' , '+City ;

    var str = '' ;
    str +=  '<div class="product-listview--resultNumber">' ;
    str +=  '  <h2 class="text-center"><span class="text-primary">'+ViewCount+'</span> '+hits_for+' '+Country+' '+City+'</h2>' ;
    str +=  '</div>' ;
    str +=  '<div class="row">' ;
    str +=  ' <div class="col-xs-6">' ;
    str +=  '   <div class="btn-toolbar sorting" role="toolbar">' ;
    str +=  '     <div class="btn-group">' ;
    str +=  '       <button class="btn btn-default btn-lg dropdown-toggle p text-primary" type="button" data-toggle="dropdown" style="width: auto;">' ;

    if( sort == 'pasc' )
      str +=  '         '+sort_title+' '+price_low_to_high+'<span class="glyphicon glyphicon-chevron-down"></span> ' ;
    else if( sort == 'pdesc' )
      str +=  '         '+sort_title+' '+price_high_to_low+'<span class="glyphicon glyphicon-chevron-down"></span> ' ;
    else if( sort == 'hasc' )
      str +=  '         '+sort_title+' '+hot_low_to_high+'<span class="glyphicon glyphicon-chevron-down"></span> ' ;
    else if( sort == 'hdesc' )
      str +=  '         '+sort_title+' '+hot_high_to_low+'<span class="glyphicon glyphicon-chevron-down"></span> ' ;

    str +=  '       </button>' ;
    str +=  '       <span class="dropdown-arrow dropdown-arrow-inverse"></span>' ;
    str +=  '       <ul class="dropdown-menu dropdown-menu-right" role="menu" style="left:0;">' ;

    str +=  '         <li alt="pasc" class="product_list_sort"><a>'+price_low_to_high+'</a></li>' ;
    str +=  '         <li alt="pdesc" class="product_list_sort"><a>'+price_high_to_low+'</a></li>' ;
    str +=  '         <li alt="hasc" class="product_list_sort"><a>'+hot_low_to_high+'</a></li>' ;
    str +=  '         <li alt="hdesc" class="product_list_sort"><a>'+hot_high_to_low+'</a></li>' ;

    str +=  '       </ul>' ;
    str +=  '      </div>' ;
    str +=  '      <!-- /btn-group --> ' ;
    str +=  '    </div>' ;
    str +=  '  </div>' ;
    str +=  '  <div class="col-xs-6 sr-only">' ;
    str +=  '    <button type="button" class="btn btn-default product-listview--switchMap text-primary sorting">' ;
    str +=  '    <span class="glyphicon glyphicon-map-marker"></span> Map view</button>' ;
    str +=  '  </div>' ;
    str +=  '</div>' ;

    return str ;
  }


  this.ToolBarViewMap = function( ViewCount , Country , City  )
  {
    if( City != '')
      City = ' , '+City ;

    var str = '' ;
    str +=  '<div class="product-listview--resultNumber">' ;
    str +=  '  <h2 class="text-center"><span class="text-primary">'+ViewCount+'</span> hits for '+Country+' '+City+'</h2>' ;
    str +=  '</div>' ;
    str +=  '<div class="row">' ;
    str +=  '   <div class="col-xs-6">' ;
    str +=  '   </div>' ;
    str +=  '   <div class="col-xs-6">' ;
    str +=  '     <button type="button" class="btn btn-default product-listview--switchList text-primary sorting">' ;
    str +=  '     <span class="glyphicon glyphicon-th-list"></span> Product view</button>' ;
    str +=  '   </div>' ;
    str +=  '</div>' ;

    return str ;
  }




  this.ListProductView = function( item,guide_lang_str,comment_str,gather_type_H_str )
  {
    // make parseJSON
    item.content = jQuery.parseJSON(item.content);

    var stat = 'all' ;

    var str = '' ;
    str +=  '<article class="product-listview"><!--multiple price----> ' ;

    str +=  '    <div class="row proudctlist_row">' ;
    str +=  '      <div class="col-md-5 col-sm-5">' ;

    str +=  '       <a href="'+item.url+'" target="_blank">' ;
    str +=  '        <div class="img-bg" style="height: 24rem;background-size:cover;background-image:url('+item.kkday_img_url+')"> ' ;
    str +=  '          <!-- Use "img-bg" to create responsive bg image--> ' ;
    str +=  '          <img src="'+item.kkday_img_url+'"><!--Must insert this image to make space for bg image to appear -->' ;
    // str +=  '          <label class="wishlist" data-toggle="tooltip" data-placement="left" title="Tooltip on left">' ;
    // str +=  '            <input type="checkbox">' ;
    // str +=  '            <span></span> </label>' ;
    str +=  '        </div>' ;
    str +=  '       </a>' ;
    str +=  '      </div>' ;
    str +=  '      <div class="col-md-7 col-sm-7">' ;
    str +=  '        <div class="row">' ;
    str +=  '          <div class="col-xs-12">' ;
    str +=  '             <a href="'+item.url+'" target="_blank">' ;
    str +=  '               <h4 class="text-info--heavy">'+item.name+'</h4>' ;
    str +=  '             </a>' ;
    str +=  '          </div>' ;

    // if( stat == 'all' || stat == 'special' )
    //   str +=  '             <div class="col-xs-2 product-listview--highlight"></div> ' ;

    // str +=  '          <div class="col-xs-2"></div>' ;
    str +=  '        </div>' ;
    str +=  '        <div class="row product-listview--tag text-info--light">' ;
    str +=  '        <div class="col-xs-12">' ;

    var country = item.country[0].split('|||') ;
    var city    = item.city[0].split('|||');

    var cityStr='';
    for(var key in item.city){
      var city_name=item.city[key].split('|||');
      cityStr= cityStr+((cityStr=='') ? '' : ', ')+city_name[0];
    }

    // console.log("item.city:"+item.city+"/city:"+city+"/cityStr:"+cityStr);
    str +=  '          <h5 style="width: 465px;">';
    str +='             <div class="row" style=" height: 20px; width: 100%;" >';
    str +='               <span class="col-md-5">';
    str +='                 <i class="fa fa-map-marker fa-lg text-primary"></i>';
    str +='                 <span  style="padding-left: 10px;">'+country[0]+', '+cityStr+'</span>';
    str +='               </span>' ;

    // star --------------------------------------------------------------------------------------------------------------
    str +=  '             <span class="col-md-4" style="padding-right: 10px;text-align: right;">' ;

    if(item.count_rec>0){
      for(var i=1;i<=Math.round(item.ave_scores);i++){
          str +=  '         <i class="fa fa-star text-muted text-primary"></i> ' ;
      }
      for(j=1;j<=5-Math.round(item.ave_scores);j++){
         str +=  '         <i class="fa fa-star text-muted"></i> ' ;
      }
      // for (var i = 1 ; i <= 5 ; i++ )
      // {
      //   if( i >= item.ave_scores )
      //     str +=  '         <i class="fa fa-star text-muted"></i> ' ;
      //   else
      //     str +=  '         <i class="fa fa-star text-muted text-primary"></i> ' ;
      // }
      str +=  '   &nbsp;&nbsp;' ;
      str +=  '         <a href="'+item.url+'#comment" target="_blank">';
      str +=  '           <span class="text-primary">'+item.count_rec+'</span>'+'<span style="color:#64696b;">'+comment_str+'</span>' ;
      str +=  '         </a>';
    }
      str +=  '           </span>' ;
      // star --------------------------------------------------------------------------------------------------------------
      str +=  '      </h5>' ;

    // tour days or hours
    if(item.days+item.hours>0){
      var days_hours='';
      if(item.days < 1)
      {
        days_hours=item.hours+' '+lang_hours;
      }
      else
      {
        days_hours=item.days+' ';
        if( item.days == 1 ) days_hours+=lang_day;
        else days_hours+=lang_days;
      }
      str += '<h5><i class="fa fa-clock-o fa-lg text-primary"></i><span style="padding-left: 10px;">'+days_hours+'</span></h5>' ;
    }

     if(item.gather_type=='H'){
        str +='      <h5>';
        str +='          <i class="fa fa-cab text-primary"></i><span style="padding-left: 10px;"> '+gather_type_H_str+'</span>';
        str +='     </h5>'
     }
    // //有接送服務 ------------------------------------------------------------------------------
    // if( item.hot == '0')
    //   str += '<h5><i class="fa fa-car fa-lg text-primary"></i><span style="padding-left: 10px;">'+car_service+'</span></h5>' ;

    // str +=  '            <h5><span class="text-primary">'+lang_supplier+'</span> '+item.supplier+'</h5>' ;

    // guide lang

    // 先暫時隱藏 command and star
    //product's command and star
    // str +=  '<h5>' ;
    // for (var i = 1 ; i <= 5 ; i++ )
    // {
    //   if( i <= item.ave_scores )
    //       str +=  '             <i class="fa fa-star fa-lg text-primary"></i> ' ;
    //   else
    //       str +=  '             <i class="fa fa-star fa-lg text-muted"></i> ' ;
    // };

    // str +=  '             ('+item.count_rec+')' ;
    // str +=  '          </h5>' ;
    str +=  '       </div>' ;
    str +=  '        </div>' ;
    str +=  '        <div class="product-listview--intro text-info--light">' ;
    str +=  '        <div class="">' ;
    // str +=  '          <p>'+item.descript+'</p>' ;
    str +=  '        </div>' ;
    str +=  '        </div>' ;

    if ( 'undefined' != typeof(item.guide_lang)  )
    {
      if(  item.guide_lang.length>0 )
      {
         str +=  '        <div class="row text-info--light">' ;
         str +=  '          <div class="col-xs-12 guide_lang_image">' ;
         str +=  '            <span class="h5" >'+guide_lang_str+'</span>';

        for (var i = 0 ; i < item.guide_lang.length ; i++)
        {
          var img_name = 'zh-cn' ;

          if(sessionLang=='zh-tw'){
            img_name = 'zh-tw';
          }

          if( guide_lang_key == 'zh-cn' )
          {
            if(item.guide_lang[i] == '繁體中文' )   img_name = 'zh-cn' ;
            if(item.guide_lang[i] == '简体中文' )   img_name = 'zh-cn' ;
          }
          else
          {
            if(item.guide_lang[i] == '繁體中文' )   img_name = 'zh-tw' ;
            if(item.guide_lang[i] == '简体中文' )   img_name = 'zh-tw' ;
          }
          // if(item.guide_lang[i] == '中文' )   img_name = 'zh-cn' ;
          if(item.guide_lang[i] == 'English' )   img_name = 'en' ;
          if(item.guide_lang[i] == '日本語'   )   img_name = 'ja' ;
          if(item.guide_lang[i] == '조선말'    )   img_name = 'ko' ;
          if(item.guide_lang[i] == '한국어'    )   img_name = 'ko' ;
          if(item.guide_lang[i] == 'Français'    )   img_name = 'fr' ;
          if(item.guide_lang[i] == 'Español'    )   img_name = 'es' ;
          if(item.guide_lang[i] == 'Deutsch'    )   img_name = 'de' ;
          if(item.guide_lang[i] == 'Italiano'    )   img_name = 'it' ;
          if(item.guide_lang[i] == 'Português'    )   img_name = 'pt' ;
          if(item.guide_lang[i] == 'العربية'    )   img_name = 'ar' ;
          if(item.guide_lang[i] == 'русский'    )   img_name = 'ru' ;

          if( img_name != '' )
            str +=  '&nbsp;<img class="guide_lang_img" src="'+webhost+'assets/img/'+img_name+'.png" data-toggle="tooltip" data-placement="top" title="'+item.guide_lang[i]+'">' ;

        };
        str +=  '          </div>' ;
      }
    }

    // 暫時不顯示供應商
    //str +=  '            <h5><span class="text-primary">'+lang_supplier+'</span> '+item.supplier+'</h5>' ;
    //
    str +=  '          <div class="col-sm-6" style="position: absolute;right:10px;bottom:10px;text-align: right;margin-right: 15px;">' ;
    str +=  '            <h4 class="text-primary" style="margin:0px;">'+item.currency+'</h4>' ;
    str +=  '            <h2 class="text-primary" style="margin:0px;">'+ priceFormat(item.price, ccy) +'</h2>' ;
    str +=  '          </div>' ;
    str +=  '        </div>' ;
    str +=  '      </div>' ;
    str +=  '    </div>' ;

    // if( parseInt( Object.size(item.content.productPkg) ) > 0 )
    // {
    //   str +=  '    <div class="container product-listview--morePrice">' ;
    //   str +=  '      <div class="row">' ;
    //   str +=  '        <button type="button" class="btn btn-default" data-toggle="collapse" data-target="#morePrice'+ item.id +'"> ' ;
    //   str +=  '         More price <span class="glyphicon glyphicon-chevron-down"></span>' ;
    //   str +=  '         </button>' ;
    //   str +=  '      </div>' ;
    //   str +=  '      <div id="morePrice'+item.id+'" class="collapse"><!-- add .in to set open by default-->' ;
    //   //console.log( item.content.productPkg ) ;
    //   $.each( item.content.productPkg , function(index, val) {
    //     // make str
    //     str +=  '        <div class="row">' ;
    //     str +=  '          <div class="col-md-8 col-sm-8">' ;
    //     str +=  '            <p>'+val.pkgDesc+'</p>' ;
    //     str +=  '          </div>' ;
    //     str +=  '          <div class="col-md-4 col-sm-4">' ;
    //     str +=  '            <h4 class="text-primary">'+val.price1+'</h4>' ;
    //     str +=  '          </div>' ;
    //     str +=  '        </div>' ;
    //   });

    //   str +=  '      </div>' ;
    //   str +=  '    </div>' ;
    // }
    str +=  '  </article>' ;
    //return
    return str ;
  }



  this.PageBarView = function( thispage , allpagecount )
  {

    var str = '' ;
    if(allpagecount!=''){
      var endPageNum=parseInt(thispage)+5;
      var starPageNum=thispage-5;

      if(parseInt(thispage)<6){
        starPageNum=1;
        endPageNum=11;
      }

      if(endPageNum>allpagecount){
        starPageNum=parseInt(starPageNum)-(endPageNum-allpagecount-1);
      }

      // console.log("starPageNum:"+starPageNum+"/thispage:"+thispage+"/endPageNum:"+endPageNum);
      str +=  '<ul class="pagination">' ;
      if(thispage>1)
        str +=  '  <li class="a-page" name="prePage"><a class="toPage">«</a></li>' ;
      //page for w
      for (var i = starPageNum ; i < endPageNum  ; i++)
      {

        if ( i > 0 && i <= allpagecount )
        {
          if( thispage == i )
            str +=  '<li class="a-page active disabled" name="' + i + '"><a>'+i+'</a></li>' ;
          else
            str +=  '<li class="a-page" name="' + i + '"><a class="toPage">'+i+'</a></li>' ;
        }
      };
      if(thispage<allpagecount)
        str +=  '  <li class="a-page" name="postPage"><a class="toPage">»</a></li>' ;
      str +=  '</ul>' ;
    }
    return str ;
  }



      // <script type="text/javascript">
      // $(function () {
      //   var product = new ProductItem() ;
      //   $('#mainContent').append( product.ToolBarView ) ;
      //   $('#mainContent').append( product.ListProductView ) ;
      // }) ;
      // </script>

}
// ----------------------------------------------------------------------------------------------------------------------------------------






function productBlock(productList, ccy, filter, kkSiteUrl, kkBaseUrl, wording, sessionLang)
{
  var str = "";
  for(var i=0; i < productList.length; i++)
  {
      var selected = false;
      if(filter.length > 0 && filter != "all")
      {
        //For location
        var productCityList = productList[i]["productCityList"];
        for(var j = 0; j < productCityList.length; j++)
        {
          if(productCityList[j]["countryCd"] == filter)
          {
            selected = true;
            break;
          }
        }
      }

      if(filter == "all" || selected)
      {
        str +=  "<article class='product-listview' id='" + productList[i]["product"]["prodOid"] + "'>" ;
        if(productList[i]["product"]["isSolr"])
          str +=  "  <a href='"+ kkSiteUrl+"/product/index/"+productList[i]["product"]["prodOid"] + "'>" ;
        else
          str +=  "<a href='#' onclick='return false'>";
        str +=  "    <div class='row'>"  ;
        str +=  "      <div class='col-md-4 col-sm-4'>"  ;
        str +=  "        <div class='img-bg' style='background-image:url(" + productList[i]["product"]["productImgUrl"] + ");' > "  ;
        str +=  "          <!-- Use 'img-bg' to create responsive bg image-->"  ;
        str +=  "            <img src='assets/img/placeholder/placeholder_1x1.png'><!--Must insert this image to make space for bg image to appear-->"  ;
        // str +=  "            <label class='wishlist' data-toggle='tooltip' data-placement='left' title='Tooltip on left'>"  ;
        // str +=  "              <input type='checkbox' value='" + productList[i]["product"]["prodOid"] + "' class='deleteWish' checked>"  ;
        // str +=  "              <span></span>"  ;
        // str +=  "            </label>"  ;
        str +=  "        </div>"  ;
        str +=  "      </div>"  ;

        str +=  "      <div class='col-md-8 col-sm-8'>"  ;
        str +=  "        <div class='row'>"  ;
        str +=  "          <div class='col-xs-10'>"  ;
        str +=  "            <h4 class='text-info--heavy'>" + productList[i]['product']['productName'] + "</h4>"  ;
        str +=  "          </div>"  ;

        // for sepcial sign
        // if( 'special' )
        // str +=  '             <div class="col-xs-2 product-listview--highlight"></div> ' ;

        str +=  "          <div class='col-xs-2 text-right'><i class='deleteWish fa fa-trash fa-2x' style='padding-top:15px;' data-prod='" + productList[i]["product"]["prodOid"] + "'></i></div>"  ;
        str +=  "        </div>"  ;
        str +=  "        <div class='row product-listview--tag text-info--light'>"  ;
        str +=  "          <div class='col-md-12'>"  ;
        str +=  '             <div class="row">';

        //For star
        str +=  '               <div class="col-md-4 col-md-push-8 div-star">';
        str +=  '                 <span class="h5" style="font-size: 1.3rem;">';
        for (var j = 1 ; j <= 5 ; j++ )
        {
          if( j <= productList[i]["product"]["avgScores"] )
              str +=  '             <i class="fa fa-star fa-lg text-primary"></i> ' ;
          else
              str +=  '             <i class="fa fa-star fa-lg text-muted"></i> ' ;
        }
        str +=  '&nbsp;&nbsp;';
        str +=  '                   <span class="text-primary">('+productList[i]["product"]["countRec"]+')</span>'
        str +=  '                 </span>';
        str +=  '               </div>';

        // for location
        str +=  '               <div class="col-md-8 col-md-pull-4">';
        str +=  '                 <span class="h5">';
        var productCityList = productList[i]["productCityList"];
        for(var j = 0; j < 2; j++)
        {
          str +=              "<i class='fa fa-map-marker fa-lg'></i>" + productCityList[j]["country"] + ", " + productCityList[j]["city"];
          if(productCityList.length == 1)  break;
          if(j == 0) str +=     "<br/>";
        }
        str +=  '                 </span>';
        str +=  '               </div>';
        str +=  '            </div>';

        str +=  '             <div class="row">';
        str +=  '               <div class="col-md-12">';
        str +=  '                 <span class="h5"><i class="fa fa-clock-o fa-lg"></i>'  ;

        // period
        if(productList[i]["product"]["tourDays"] > 0)
          str +=               productList[i]["product"]["tourDays"] + " " + wording["days"];
        if(productList[i]["product"]["tourHours"] > 0)
          str +=               productList[i]["product"]["tourHours"] + " " + wording["Hours"];
        str +=  "                 </span>"  ;
        str +=  '               </div>';
        str +=  '            </div>';

        // for guide languages
        str +=  '           <div class="row">';
        str +=  "             <div class='col-md-12 guide_lang_image'>";
        for(var k in productList[i]["product"]["guideLangMap"]){
          if(k=='zh-tw' || k=='zh-cn' ){
            if(sessionLang=='zh-tw'){
              str +=  '<img src="'+kkBaseUrl+'assets/img/zh-tw.png'+'" data-toggle="tooltip" data-placement="bottom" title="'+productList[i]["product"]["guideLangMap"][k]+'">';
            }else{
              str +=  '<img src="'+kkBaseUrl+'assets/img/zh-cn.png'+'" data-toggle="tooltip" data-placement="bottom" title="'+productList[i]["product"]["guideLangMap"][k]+'">';
            }
          }
          else
            str +=  '<img src="'+kkBaseUrl+'assets/img/'+k+'.png'+'" data-toggle="tooltip" data-placement="bottom" title="'+productList[i]["product"]["guideLangMap"][k]+'">';
        }
        str +=  "             </div>";
        str +=  "           </div>";

        str +=  "          </div>"  ;
        str +=  "        </div>"  ;
        str +=  "        <div class='product-listview--intro text-info--light'>" ;
        str +=  "          <div class='col-md-12'>"  ;
        str +=  "            <p>" + productList[i]["product"]["productDesc"]+ "</p>"  ;
        str +=  "          </div>"  ;
        str +=  "        </div>"  ;
        str +=  "        <div class='row text-info--light'>"  ;
        str +=  "          <div class='col-sm-6'>"  ;
        // str +=  "            <h5>" + productList[i]["product"]["supplierName"] + "</h5>"  ;
        str +=  "          </div>"  ;
        str +=  "          <div class='col-sm-6'>"  ;
        str +=  "            <h4 class='text-primary'>"+ ccy + "</h4>"  ;
        str +=  "            <h2 class='text-primary'>"  ;
        str +=                  priceFormat(productList[i]["product"]["minPrice"], ccy)  ;
        str +=  "            </h2>"  ;
        str +=  "          </div>"  ;
        str +=  "        </div>"  ;
        if(productList[i]["product"]["isSolr"]){
          str +=  "        <div class='row '>"  ;
          str +=  "          <div class='col-sm-offset-8 col-sm-4 text-right'>"  ;
          str +=  "            <button type='button' class='btn btn-primary btn-lg btn-block wish_booking_btn'>" ;
          str +=                 wording["bookingStr"];
          str +=  "            </button>";
          str +=  "          </div>"  ;
          str +=  "        </div>"  ;
        }
        str +=  "      </div>"  ;
        str +=  "    </div>"  ;
        str +=  "  </a>"  ;


        //for multiple prices
        // if(productList[i]["productPkgList"].length > 1)
        // {
        //   str +=  "<div class='container product-listview--morePrice'>"  ;
        //   str +=  "   <div class='row'>"  ;
        //   str +=  "     <button type='button' class='btn btn-default' data-toggle='collapse' data-target='#morePrice" + productList[i]["product"]["prodOid"] + "'>"  ;
        //   str +=  "       More price <span class='glyphicon glyphicon-chevron-down'></span>"  ;
        //   str +=  "     </button>"  ;
        //   str +=  "   </div>"  ;
        //   str +=  "   <div id='morePrice" + productList[i]["product"]["prodOid"] + "' class='collapse morePrice'><!-- add .in to set open by default-->"  ;

        //   var productPkgList = productList[i]["productPkgList"]
        //   for(var j=0; j < productPkgList.length; j++)
        //   {
        //     str +=  "      <div class='row'>"  ;
        //     str +=  "         <div class='col-md-8 col-sm-8'>"  ;
        //     str +=  "           <p>" + productPkgList[j]['pkgName'] + "</p>"  ;
        //     str +=  "         </div>"  ;
        //     str +=  "         <div class='col-md-4 col-sm-4'>"  ;
        //     str +=  "            <h4 class='text-primary'>" + productPkgList[j]["currency"] + "&nbsp" + priceFormat(productPkgList[j]["pkgPrice"]) + "</h4>"  ;
        //     str +=  "         </div>"  ;
        //     str +=  "      </div>"  ;
        //   }

        //   str +=  "   </div>" ;
        //   str +=  "</div>";
        // }

        str +=  "</article>";
      }
    }

    return str;
}


// ----------------------------------------------------------------------------------------------------------------------------------------



// orderBlock for kkday
// 2014.09.02
//
// ----------------------------------------------------------------------------------------------------------------------------------------


function orderBlock(orderInfo, kkSiteUrl)
{
  var block =

      "<article class='product-listview'>" +
        "<a href='" + kkSiteUrl + "/order/show/" + orderInfo["orderMid"] + "' style='color:inherit;'>" +
          "<div class='row'>" +
                  "<div class='col-md-4 col-sm-4'>" +
              "<div class='img-bg' style='background-image:url(" + orderInfo["imgUrl"] + ")'><!-- Use \"img-bg\" to create responsive bg image-->" +
                      "<img src='assets/img/placeholder/placeholder_1x1.png'><!--Must insert this image to make space for bg image to appear-->" +
                    "</div>" +
                  "</div>" +

            "<div class='col-md-8 col-sm-8'>" +
              "<div class='row'>" +
                //"<div class='col-md-8 col-sm-8'>" +
                  "<h4 class='text-info--heavy'>" + orderInfo["productName"] + "</h4>" +
                  "<div class='row'>" +
                    "<div class='col-sm-3'>" +
                      "<p><i class='fa fa-calendar'></i>" + lang_common.common_order_depart_date + "</p>" +
                    "</div>" +
                    "<div class='col-sm-9'>";
                      switch(orderInfo["orderStatus"]){
                        case "GO":
                          block += "<h2 class='text-primary'>" + dateFormat(orderInfo["begLstGoDt"]) + "</h2>";
                          break;
                        case "BACK":
                          block += "<h2 class='text-muted'>" + dateFormat(orderInfo["begLstGoDt"]) + "</h2>";
                          break;
                        case "CX":
                          block += "<h2 class='text-muted'>" + lang_common.common_order_cancelled + "</h2>";
                          break;
                      }
    block +=            "</div>" +
                  "</div>" +
              "</div>" +
              "<div class='row text-info--light'>" +
                  "<div class='col-sm-6'>" +
                      "<h5>" + lang_common.common_order_purchased_on + " " + dateFormatGMT(orderInfo["crtDt"]) + "</h5>" +
                      "<h5>" + lang_common.common_order_booking_no + " " + orderInfo["orderMid"] + "</h5>" +
                  "</div>" +
                  "<div class='col-sm-6'>" +
                      "<h4 class=''>USD</h4>" +
                      "<h2 class=''>$" + numeral(orderInfo["pricePay"]).format('0,0.00') + "</h2>";

                      if(orderInfo["orderStatus"] == "CX")
                      {
                        block +=
                        "<h4 class='text-primary'>" +
                        lang_common.common_order_refund + ": $" + numeral(orderInfo["priceRefund"]).format('0,0.00') + "<br/>" +
                        lang_common.common_order_cancel_fee + ": $" + numeral((orderInfo["pricePay"] - orderInfo["priceRefund"])).format('0,0.00') +
                        "</h4>";
                      }

    block +=      "</div>" +
              "</div>" +
            "</div>" +
          "</div>" +
        "</a>" +
      "</article>";


    return block;
}


// ----------------------------------------------------------------------------------------------------------------------------------------



// msgListBlock for kkday
// 2014.09.02
//
// ----------------------------------------------------------------------------------------------------------------------------------------


function msgListBlock(msgList, filter, kkSiteUrl)
{
    var block = "";

    for(var i=0; i < msgList.length; i++)
    {
      if(filter.length == 0 || (filter.length > 0 && msgList[i]["msg"]["msgType"] == filter))
      {
        var n = subDescription(msgList[i]["msg"]["msgContent"]);

        block +=
        "<a href='" + kkSiteUrl + "/member/msg/" + msgList[i]["msg"]["msgOid"] + "' style='color:inherit'>";

          if(msgList[i]["msg"]["isRead"] == "N")
            block += "<div class='row message-title'>";
          else
            block += "<div class='row message-title read'>";

      block +=    "<div class='col-xs-2'>" +
                  "<div class='img-bg message-photo'>" +
                    "<img src='" + msgList[i]["msg"]["photoUrl"] + "'/>" +
                  "</div>" +
                "</div>" +
                "<div class='col-xs-10'>" +
                    "<div class='row'>" +
                      "<div class='col-xs-12'>" ;
                          if(msgList[i]["msg"]["isRead"] == "N")
      block +=              "<h4 class='text-info--heavy'><strong>" + msgList[i]["msg"]["msgContent"].substr(0,n) +"</strong></h4>";
                          else
      block +=              "<h4 class='text-info--heavy'>" + msgList[i]["msg"]["msgContent"].substr(0,n) +"</h4>";

      block +=        "</div>" +
                    "</div>" +
                    "<div class='row'>" +
                      "<div class='col-xs-6'>" +
                          "<h5>" + msgList[i]["msg"]["name"] + "</h5>" +
                      "</div>" +
                      "<div class='col-xs-6'>" +
                          "<h5>" +msgList[i]["msg"]["userMsgDt"]  + "</h5>" +
                      "</div>" +
                    "</div>" +
                "</div>" +
              "</div>" +
          "</a><!-- / of message-title 01-->";
      }

    }


    return block;

}



// ----------------------------------------------------------------------------------------------------------------------------------------



// messageBlock for kkday
// 2014.09.02
//
// ----------------------------------------------------------------------------------------------------------------------------------------

function messageBlock(msgList, memberImg, defaultImgM, defaultImgS)
{
  var block = "";

  if(memberImg == "") memberImg = defaultImgM;

  for(var i=0; i < msgList.length; i++)
  {
      block += '  <div class="row message">';
      block += '      <div class="col-sm-12">';
      block += '         <div class="box">';

                          if(msgList[i]["msg"]["sendType"] == "M") {
      block += '            <div class="editer float-left">';
      block += '                <div class="editer-logo" style="background-image:url(' + memberImg + ')"></div>';
      block += '                <div class="text-center">' + msgList[i]["msg"]["name"] + '</div>';
      block += '            </div>';
                          } else {
      block += '            <div class="editer float-right">';
                                if(msgList[i]["msg"]["photoUrl"] == '') {
      block += '                  <div class="editer-logo" style="background-image:url(' + defaultImgS + ')"></div>';
                                } else {
      block += '                  <div class="editer-logo" style="background-image:url(' + msgList[i]["msg"]["photoUrl"] + ')">';
      block += '                    <img src="' + msgList[i]["msg"]["photoUrl"] + '" onerror="this.parentNode.style.backgroundImage = \'url(' + defaultImgS + ')\'" style="display:none;">';
      block += '                  </div>';
                                }
      block += '                <div class="text-center">' + msgList[i]["msg"]["name"] + '</div>';
      block += '            </div>';
                          }
      block += '            <div class="content">';
      block +=                  msgList[i]["msg"]["msgContent"];
      block += '            </div>';
      block += '        </div>';

                          if(msgList[i]["msg"]["sendType"] == "M")
      block += '              <div class="date text-muted text-right">';
                          else
      block += '              <div class="date text-muted text-left">';
      block +=              msgList[i]["msg"]["userMsgDt"];
      block += '        </div>';
      block += '        <hr />';
      block += '    </div>';
      block += '</div>';
  }

  return block;
}



// ----------------------------------------------------------------------------------------------------------------------------------------



// pageBtnGroup for kkday
// 2014.09.02
//
// ----------------------------------------------------------------------------------------------------------------------------------------


function pageBarView( thispage , allpagecount )
{
  var str = '' ;

  var endPageNum=parseInt(thispage)+5;
  var starPageNum=thispage-5;

  if(parseInt(thispage)<6){
    starPageNum=1;
    endPageNum=11;
  }

  if(endPageNum>allpagecount){
    starPageNum=parseInt(starPageNum)-(endPageNum-allpagecount-1);
  }

  str +=  '<ul class="pagination">' ;
  str +=  '  <li class="a-page" name="prePage"><a>«</a></li>' ;
  //page for w
  for (var i = starPageNum; i < endPageNum  ; i++) {
    if ( i > 0 && i <= allpagecount )
      str +=  '<li class="a-page" name="' + i + '"><a>'+i+'</a></li>' ;
  };
  str +=  '  <li class="a-page" name="postPage"><a>»</a></li>' ;
  str +=  '</ul>' ;
  return str ;
}



// ----------------------------------------------------------------------------------------------------------------------------------------



// dateFormat for kkday
// 2014.09.02
//
// ----------------------------------------------------------------------------------------------------------------------------------------

function dateFormat(dateInput)
{
  var year = dateInput.substr(0, 4);
  var month = dateInput.substr(5, 2);
  var day = dateInput.substr(8, 2);
  var monthName = [ 'Jan' , 'Feb' , 'Mar' , 'Apr' , 'May' , 'Jun' , 'Jul' , 'Aug' , 'Sep' , 'Oct' , 'Nov' , 'Dec'  ] ;

  return day + " " + monthName[parseInt(month) - 1] + " " + year;
}

function dateFormatGMT(dateInput, withLocation)
{
  var year = dateInput.substr(0, 4);
  var month = dateInput.substr(5, 2);
  var day = dateInput.substr(8, 2);
  var monthName = [ 'Jan' , 'Feb' , 'Mar' , 'Apr' , 'May' , 'Jun' , 'Jul' , 'Aug' , 'Sep' , 'Oct' , 'Nov' , 'Dec'  ] ;

  if(withLocation)
    return day + " " + monthName[parseInt(month) - 1] + " " + year + dateInput.substr(10);
  else
    return day + " " + monthName[parseInt(month) - 1] + " " + year;
}


function priceFormat(price, ccy)
{
  var withDecimal = ["AUD","NZD","USD","GBP","EUR"];
  var intCase = ['KRW', 'JPY'];

  if(_.includes(withDecimal, ccy)) {
	  return numeral(price).format('0,0.00');
  } else if (_.includes(intCase, ccy)) {
	  return numeral(_.floor(price)).format('0,0');
  }else {
	  return numeral(_.round(price)).format('0,0');
  }
}

function priceUnformat(price){
  return numeral().unformat(price);
}

function convertUSDCurrency(USDPrice, exchangeRate, ccy) {
  return priceFormat(USDPrice * exchangeRate, ccy);
}

function loginHeader(member,defaultImg,msglistUrl,isHome)
{
    $('#account-logIn').modal('hide');
    $('#errorMessage').html("");

    //reset input value
    $("#loginEmail").val("");
    $("#loginPassword").val("");

    //account div

    var accountDiv = "";
    var displayName = (member.firstName ? member.firstName : member.email);

    accountDiv += '     <div class="table-row">';
    accountDiv += '       <div class="table-cell border-left table-account" style="line-height: initial;" data-toggle="modal" data-target="#myAccount">';
                            if(member.photoUrl)
    accountDiv += '           <div class="account-user-photo" id="account-user-photo" style="background-image:url(\'' + member.photoUrl + '\')"></div>';
                            else
    accountDiv += '           <div class="account-user-photo" id="account-user-photo" style="background-image:url(\'' + defaultImg + '\')"></div>';
    accountDiv += '           <div class="account-user-name" id="account-user-name" style="line-height: 65px;">' + displayName + '</div>';
    accountDiv += '       </div>';
    accountDiv += '       <a href="'+msglistUrl+'">';
    accountDiv += '          <div class="table-cell border-double account-user-msg" style="line-height: initial;">';
    accountDiv += '             <span class="glyphicon glyphicon-bell"></span>';
                                if(member.unreadMsg)
    accountDiv += '             <div class="account-user-msg-count text-center">'+member.unreadMsg+'</div>';
    accountDiv += '          </div>';
    accountDiv += '       </a>';
    accountDiv += '     </div>';

    $("#header-accountDiv").html(accountDiv);
}


/**
 * [isNumberKey : number only ]
 * @param  {[type]}  evt [description]
 * @return {Boolean}     [description]
 */
function isNumberKey(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}



/**
 * [size description]
 * @param  {[type]} obj [description]
 * @return {[type]}     [description]
 */
Object.size = function(obj) {
    var size = 0, key;
    for (key in obj) {
        if (obj.hasOwnProperty(key)) size++;
    }
    return size;
};



/**
 * [size description]
 * @param  {[type]} obj [description]
 * @return {[type]}     [description]
 */
 function subDescription(string)
 {
    var utf8length = 0;
    var n = 0;

    for (n = 0; n < string.length; n++) {
      var c = string.charCodeAt(n);
      if (c < 128)
        utf8length++;
      else
        utf8length = utf8length + 2;

      if(utf8length > 90) break;
    }

    return n;
 }


//count string length with utf8
 function getUTF8Length(s) {
     var len = 0;
     for (var i = 0; i < s.length; i++) {
         var code = s.charCodeAt(i);
         if (code <= 0x7f) {
           len += 1;
         } else if (code <= 0x7ff) {
           len += 2;
         } else if (code >= 0xd800 && code <= 0xdfff) {
   // Surrogate pair: These take 4 bytes in UTF-8 and 2 chars in UCS-2
   // (Assume next char is the other [valid] half and just skip it)
           len += 4; i++;
         } else if (code < 0xffff) {
           len += 3;
         } else {
           len += 4;
         }
     }
     return len;
 }

// check image load status
function checkImage(imageUrl, callback) {
  var img = new Image();
  img.onload = function() {
    callback(true);
  };
  img.onerror = function() {
    callback(false);
  };
  img.src = imageUrl;
}

// now can use $.QueryString to get query of currentPage href
(function($) {
	$.QueryString = (function(a) {
		if (a == "") return {};
		var b = {};
		for (var i = 0; i < a.length; ++i) {
			var p = a[i].split('=');
			if (p.length != 2) continue;
			b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
		}
		return b;
	})(window.location.search.substr(1).split('&'));
})(jQuery);
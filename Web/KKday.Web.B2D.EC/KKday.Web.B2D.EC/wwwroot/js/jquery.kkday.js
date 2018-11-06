/*
    kkday Jquery Plugin . DIY
*/

// messageBlock for kkday
// 2014.09.02
//
// ----------------------------------------------------------------------------------------------------------------------------------------

function messageBlock(msgList, defaultImgM, defaultImgS)
{
  var block = "";
  var defaultImg = '';

  for(var i=0; i < msgList.length; i++)
  {
      block += '  <div class="row message">';
      block += '      <div class="col-sm-12">';
      block += '         <div class="box">';

                          if(msgList[i]["msg"]["sendType"] == "M") {
                            defaultImg = defaultImgM;
      block += '            <div class="editer float-left">';
                          } else {
                            defaultImg = defaultImgS;
      block += '            <div class="editer float-right">';
                          }
      block += '              <div class="editer-logo" style="background-image:url(' + msgList[i]["msg"]["show_logo_path"] + ')">';
      block += '                <img src="' + msgList[i]["msg"]["show_logo_path"] + '" onerror="this.parentNode.style.backgroundImage = \'url(' + defaultImg + ')\'" style="display:none;">';
      block += '              </div>';

      block += '              <div class="text-center">' + msgList[i]["msg"]["show_name"] + '</div>';
      block += '            </div>';

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

function formatWeekDay(weekDays, weekDayFormat) {
  weekDayFormat = weekDayFormat || ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

  var openWeekDays = [];
  _.each(_.split(weekDays, ','), function(isOpen, weekDay) {
    if(isOpen == 'Y') {
      openWeekDays.push(weekDayFormat[weekDay]);
    }
  });

  return _.join(openWeekDays, ', ');
}

function formatTime(time) {
  return _.padStart(time.hour, 2, '0') + ':' + _.padStart(time.minute, 2, '0');
}

function priceFormat(price, ccy)
{
  var withDecimal = ["AUD", "NZD", "USD", "GBP", "EUR", "CAD"];
  var intCase = ['KRW', 'JPY'];

  if(_.includes(withDecimal, ccy)) {
      return numeral(price).format('0,0.00');
  } else if (_.includes(intCase, ccy)) {
      return numeral(_.floor(price)).format('0,0');
  }else {
      return numeral(_.round(price)).format('0,0');
  }
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

function dateFormatWithPattern(date, pattern) {
  if (!date && date.length != 8) {
    return date;
  }

  pattern = pattern || 'YYYY-MM-DD';
  return moment(date.substr(0, 4) + '-' + date.substr(4, 2) + '-' + date.substr(6, 2))
    .format(pattern);
}

function printf(template, args) {
  var length = (args == null) ? 0 : args.length;
  if (template != null && length) {
    var result = _.replace(template, '%s', args[0]);

    return printf(result, _.tail(args));
  }

  return template;
}

function removeQueryStringParam(key, url) {
  if(!url) {
    url = window.location.href;
  }

  var hashParts = url.split('#');
  var regex = new RegExp("([?&])" + key + "=.*?(&|#|$)", "i");
  if(hashParts[0].match(regex)) {
      url = hashParts[0].replace(regex, '$1');
      url = url.replace(/([?&])$/, '');
      if (typeof hashParts[1] !== 'undefined' && hashParts[1] !== null) {
        url += '#' + hashParts[1];
      }
  }

  return url;
}

function updateQueryStringParam(uri, key, value) {
  var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
  var separator = uri.indexOf('?') !== -1 ? "&" : "?";
  if (uri.match(re)) {
    return uri.replace(re, '$1' + key + "=" + value + '$2');
  }
  else {
    return uri + separator + key + "=" + value;
  }
}

function getParameterByName(name, url) {
  if (!url) url = window.location.href;
  name = name.replace(/[\[\]]/g, '\\$&');
  var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
      results = regex.exec(url);
  if (!results) return null;
  if (!results[2]) return '';
  return decodeURIComponent(results[2].replace(/\+/g, ' ')) || '';
}

function formatEventOptions(eventValue) {
    if(_.isEmpty(eventValue)) {
        return [];
    }
    var eachEventData = eventValue.split(',');

    return _.reduce(eachEventData, function(result, value, key) {
        var decodedData = value.split('_');

        result.push({
            "id": decodedData[0],
            "eventTime": decodedData[1],
            "eventQuantity": decodedData[2]
        });
        return result;
  }, []);
}

function caculateAge(compareDay, birthDay) {
  var compareDay = new Date(compareDay);
  var birthDay = new Date(birthDay);
  var age = compareDay.getFullYear() - birthDay.getFullYear();
  var month = compareDay.getMonth() - birthDay.getMonth();
  if (month < 0 || (month === 0 && compareDay.getDate() < birthDay.getDate())) {
    age--;
  }

  return age;
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

    $.reloadWithHashRemoved = (function(l) {
        return function() {
            l.href = l.origin + l.pathname + l.search;
        };
    })(window.location);

    $.historyReloader = (function(h, l) {
        var pageShowRegistered = false;

        return {
            setStateShouldReload: function(shouldReload) {
                // 有些頁面用字串當state
                // 如果在這些頁面使用的話預設不覆寫
                var newState = h.state;

                if(_.isObject(newState)) {
                    newState = _.assign({}, newState, {
                        shouldReload: shouldReload
                    });
                } else if(_.isNull(newState)) {
                    newState = {
                        shouldReload: shouldReload
                    };
                }

                h.replaceState(
                    newState,
                    document.title,
                    l.href
                );

                return this;
            },
            shouldReload: function() {
                var state = h.state;
                return state && _.isObject(state) && state.shouldReload;
            },
            registerPageShowEvent: function() {
                if(pageShowRegistered) {
                    return this;
                }

                var self = this;

                $(window).on('pageshow', function(e) {
                    if(self.shouldReload()) {
                        self.setStateShouldReload(false);
                        l.reload();
                    }
                });

                pageShowRegistered = true;
                return this;
            }
        };
    })(window.history, window.location);
})(jQuery);

$.historyReloader.registerPageShowEvent();

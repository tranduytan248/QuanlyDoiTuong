<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl js" xmlns="http://www.w3.org/1999/xhtml" xmlns:js="urn:custom-javascript" xmlns:ds="http://www.w3.org/2000/09/xmldsig#">
     <xsl:template name="formatSerial">
          <xsl:variable name="pPattern">
               <xsl:value-of select="substring(../../../TTChung//KHMSHDon,1,1)" />
          </xsl:variable>
          <xsl:variable name="pSerial">
               <xsl:value-of select="../../../TTChung//KHHDon" />
          </xsl:variable>
          <xsl:value-of select="concat($pPattern,$pSerial)" />
     </xsl:template>
     <!--định dạng thời gian ký-->
     <xsl:template name="formatdateSign">
          <xsl:param name="DateTimeStr" />
          <xsl:variable name="datestr">
               <xsl:value-of select="substring-before($DateTimeStr,'T')" />
          </xsl:variable>
          <xsl:variable name="timestr">
               <xsl:value-of select="substring-after($DateTimeStr,'T')" />
          </xsl:variable>
          <xsl:variable name="mm">
               <xsl:value-of select="substring($datestr,6,2)" />
          </xsl:variable>
          <xsl:variable name="dd">
               <xsl:value-of select="substring($datestr,9,2)" />
          </xsl:variable>
          <xsl:variable name="yyyy">
               <xsl:value-of select="substring($datestr,1,4)" />
          </xsl:variable>
          <xsl:choose>
               <xsl:when test="$mm != '' ">
                    <xsl:value-of select="concat($dd,'/',$mm, '/', $yyyy, ' ', $timestr)" />
               </xsl:when>
          </xsl:choose>
     </xsl:template>
     <xsl:template name="addfinalbodyTT78">
          <style>label{color: #cc3333  !important}</style>
          <div class="statistics"></div>
          <div class="clearfix" />
          <!--panel replace-->
          <xsl:choose>
               <xsl:when test="../../../TTChung/TTHDLQuan !=''">
                    <div style="text-align:center;padding-top:0px;font-size:11px;text-transform:uppercase;font-weight: bold;margin-top:5px;color: #cc3333 ;">
                         <xsl:value-of select="../../../TTChung/TTHDLQuan/GChu" />
                    </div>
               </xsl:when>
          </xsl:choose>
          <!---->
          <div class="clearfix" />
          <div class="clearfix">
               <!--variable-->
               <xsl:variable name="serial">
                    <xsl:value-of select="SerialNo" />
               </xsl:variable>
               <xsl:variable name="pattern"></xsl:variable>
               <xsl:variable name="invno">
                    <xsl:value-of select="InvoiceNo" />
               </xsl:variable>
               <!---->
               <center>
                    <div class="">
                         <table style="width:100%" cellspacing="0" cellpadding="0" border="0">
                              <tr style="vertical-align: top;">
                                   <!--panel client-->
                                   <td width="23%" style="border-left: 0px !important;border-right: 0px !important;color: #000;">
                                        <div class="payment fl-l" style="width: 100%; float:left; text-align: center;margin-top: 0px;">
                                             <div>
                                                  <div style="text-align: center;">
                                                       <p style="font-size: 13px; margin: 0px; text-align: center;">
                                                            <b style="color: #cc3333 ;">Người mua hàng</b>
                                                       </p>
                                                       <span style="font-size: 13px; text-align: center;">
                                                            <i style="color: #cc3333 ;">(Ký, ghi rõ họ tên)</i>
                                                       </span>
                                                       <p style="margin-top:70px !important;">                                                </p>
                                                  </div>
                                             </div>
                                        </div>
                                   </td>
                                   <xsl:choose>
                                        <xsl:when test="../../../../convert!=''">
                                             <td width="25%" style="border-left: 0px !important;border-right: 0px !important;color: #000;">
                                                  <div class="payment fl-l" style="width: 100%; float:left; text-align: center;margin-top: 1px;font-size:13px;color: #cc3333 ;">
                                                       <label style="font-size:13px;margin-top: 0px;margin-bottom: 0px;color: #cc3333 ;">
                                                            <b>
                                                                 <!--<xsl:value-of select="/Invoice/convert"/>-->                          HÓA ĐƠN CHUYỂN ĐỔI TỪ HÓA ĐƠN ĐIỆN TỬ
                                                            </b>
                                                       </label>
                                                       <p style="font-size:13px; margin:0px">
                                                            Ngày
                                                            <b>
                                                                 <xsl:value-of select="substring(../../../../ConvertDate,1,2)" />
                                                            </b> tháng
                                                            <b>
                                                                 <xsl:value-of select="substring(../../../../ConvertDate,4,2)" />
                                                            </b> năm
                                                            <b>
                                                                 <xsl:value-of select="concat('20',substring(../../../../ConvertDate,9,2))" />
                                                            </b>
                                                       </p>
                                                       <p style="font-size:13px; margin:0px">                        Người chuyển đổi                      </p>
                                                       <i>(Ký, ghi rõ họ tên)</i>
                                                  </div>
                                             </td>
                                        </xsl:when>
                                   </xsl:choose>
                                   <!--panel thuế-->
                                   <td width="" style="border-left: 0px !important;border-right: 0px !important;text-align: center;padding-left: 0px;padding-right: 0px;">
                                        <xsl:choose>
                                             <xsl:when test="../../../../DSCKS/CQT">
                                                  <p style="font-size:13px; margin-top:0px;color: #000000;margin-top:1px;margin-bottom: 0px">
                                                       <b style="color: #cc3333 ;">                        CƠ QUAN THUẾ                      </b>
                                                  </p>
                                                  <i style="color: #cc3333 ;font-size: 13px;">(Ký, đóng dấu)</i>
                                                  <xsl:choose>
                                                       <xsl:when test="../../../../image != '' ">
                                                            <div class="bgimg" style="background:url({../../../../image/@URI}) no-repeat center center; height: 115px;width: 98%;">
                                                                 <p style="margin-top:3px;margin-bottom:2px;font-size:13px">
                                                                      <xsl:value-of select="../../../../image" />
                                                                 </p>
                                                                 <p style="font-size:13px">
                                                                      Ký bởi:
                                                                      <xsl:value-of select="//*[contains(@Id,'Tct-')]//*[local-name() = 'X509SubjectName']" /><br />                            Ký ngày:
                                                                      <xsl:call-template name="formatdateSign">
                                                                           <xsl:with-param name="DateTimeStr" select="//*[contains(@Id,'Tct-')]//*[local-name() = 'SigningTime']" />
                                                                      </xsl:call-template>
                                                                 </p>
                                                            </div>
                                                       </xsl:when>
                                                  </xsl:choose>
                                             </xsl:when>
                                        </xsl:choose>
                                   </td>
                                   <!--panel server-->
                                   <td width="28%" style="border-left: 0px !important;border-right: 0px !important;">
                                        <div class="payment fl-l" style="width:100%;float:left; text-align: center;margin-top: 1px;margin-bottom: 0px;">
                                             <div>
                                                  <div style="text-align: center;">
                                                       <p style="font-size: 13px; margin: 0px; text-align: center;">
                                                            <b style="color: #cc3333 ;">Người bán hàng</b>
                                                       </p>
                                                       <span style="font-size: 13px; text-align: center;">
                                                            <i style="color: #cc3333 ;font-size: 13px;">                          (Ký, ghi rõ họ tên)                        </i>
                                                       </span>
                                                  </div>
                                             </div>
                                        </div>
                                        <div class="date fl-r" style="margin-top:0px;float: left; margin-right:0px; width:100% !important;    height: 100px;">
                                             <p style="font-size:13px; margin:0px">
                                                  <xsl:choose>
                                                       <xsl:when test="../../../../image != '' ">
                                                            <div class="bgimg" style="background:url({../../../../image/@URI}) no-repeat center center; height: 83px;width: 98%;" onclick="showDialog('dialogServer','{$serial}','{$pattern}','{$invno}',0,'messSer','is')">
                                                                 <p style="margin-top:3px;margin-bottom:3px;font-size:13px;margin-left: 1px;">
                                                                      <xsl:value-of select="../../../../image" />
                                                                 </p>
                                                                 <p style="font-size:13px;margin-left: 1px;">
                                                                      Ký bởi:
                                                                      <xsl:value-of select="../../NBan/Ten" /><br />                            Ký ngày:
                                                                      <xsl:call-template name="formatdateSign">
                                                                           <xsl:with-param name="DateTimeStr" select="../../../../DSCKS/NBan/ds:Signature/ds:Object/SignatureProperties/SignatureProperty/SigningTime" />
                                                                      </xsl:call-template>
                                                                 </p>
                                                            </div>
                                                       </xsl:when>
                                                  </xsl:choose>
                                             </p>
                                        </div>
                                   </td>
                              </tr>
                         </table>
                    </div>
               </center>
               <!--dialog server-->
               <div id="dialogServer" style="background-color:white;display:none">
                    <xsl:variable name="sc">
                         <xsl:value-of select="//*[contains(@Id,'serSig')]//*[local-name() = 'X509Certificate']" />
                    </xsl:variable>
                    <div style="color:blue" id="messSer">Unknown!</div>
                    <br />
                    <br />
                    <a href="#" onclick="displayCert('{$sc}')">
                         <div style="color:#184D4E">Xem thông tin chứng thư</div>
                    </a>
               </div>
               <!--dialog client-->
               <div id="dialogClient" style="background-color:white;display:none">
                    <xsl:variable name="sc1">
                         <xsl:value-of select="//*[contains(@Id,'cltSig')]//*[local-name() = 'X509Certificate']" />
                    </xsl:variable>
                    <div style="color:blue" id="messClt">Unknown!</div>
                    <br />
                    <br />
                    <a href="#" onclick="displayCert('{$sc1}')">
                         <div style="color:#184D4E">Xem thông tin chứng thư</div>
                    </a>
               </div>
               <!---->
               <!---->
               <xsl:choose>
                    <xsl:when test="../../../TTChung/DDTCuu !=''">
                         <center style="margin-top:5px;font-style: italic;">
                              <label>
                                   <b style="padding-left:5px;">Tra cứu hóa đơn tại website:</b>
                                   <span>
                                        <xsl:value-of select="../../../TTChung/DDTCuu" />
                                   </span>
                              </label>
                              <xsl:choose>
                                   <xsl:when test="../../../TTChung/MTCuu !=''">
                                        <label>
                                             <b>Mã tra cứu:</b>
                                             <b>
                                                  <xsl:value-of select="../../../TTChung/MTCuu" />
                                             </b>
                                        </label>
                                   </xsl:when>
                              </xsl:choose>
                         </center>
                    </xsl:when>
               </xsl:choose>
               <!--tra cứu-->
          </div>
          <!--panel convert-->
     </xsl:template>
     <!--Phần ký số-->
     <xsl:variable name="itemsPerPage">
          <xsl:value-of select="15" />
     </xsl:variable>
     <xsl:variable name="itemCount">
          <xsl:value-of select="count(HDon//DLHDon//NDHDon//DSHHDVu//HHDVu)" />
     </xsl:variable>
     <xsl:variable name="pagesNeeded">
          <xsl:choose>
               <xsl:when test="$itemCount &lt;= $itemsPerPage">
                    <xsl:value-of select="1" />
               </xsl:when>
               <xsl:otherwise>
                    <xsl:choose>
                         <xsl:when test="$itemCount mod $itemsPerPage = 0">
                              <xsl:value-of select="$itemCount div $itemsPerPage" />
                         </xsl:when>
                         <xsl:otherwise>
                              <xsl:value-of select="ceiling($itemCount div $itemsPerPage)" />
                         </xsl:otherwise>
                    </xsl:choose>
               </xsl:otherwise>
          </xsl:choose>
     </xsl:variable>
     <xsl:template name="addZero">
          <xsl:param name="count" />
          <xsl:if test="$count &gt; 0">
               <xsl:text>0</xsl:text>
               <xsl:call-template name="addZero">
                    <xsl:with-param name="count" select="$count - 1" />
               </xsl:call-template>
          </xsl:if>
     </xsl:template>
     <xsl:template name="addDots">
          <xsl:param name="val" />
          <xsl:param name="val1" />
          <xsl:param name="val2" />
          <xsl:param name="i" select="1" />
          <xsl:if test="$val1&gt;0">
               <xsl:choose>
                    <xsl:when test="$val2 !=0">
                         <xsl:value-of select="substring($val,$i,$val2)" />
                         <xsl:if test="substring($val,$i+$val2+1,1) !=''">
                              <xsl:text>.</xsl:text>
                         </xsl:if>
                         <xsl:call-template name="addDots">
                              <xsl:with-param name="val" select="$val" />
                              <xsl:with-param name="val1" select="$val1 - 1" />
                              <xsl:with-param name="i" select="$i + $val2" />
                              <xsl:with-param name="val2" select="3" />
                         </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                         <!--<xsl:text>test</xsl:text>-->
                         <xsl:value-of select="substring($val,$i,3)" />
                         <xsl:if test="substring($val,$i+3,1) !=''">
                              <xsl:text>.</xsl:text>
                         </xsl:if>
                         <xsl:call-template name="addDots">
                              <xsl:with-param name="val" select="$val" />
                              <xsl:with-param name="val1" select="$val1 - 1" />
                              <xsl:with-param name="i" select="$i + 3" />
                              <xsl:with-param name="val2" select="3" />
                         </xsl:call-template>
                    </xsl:otherwise>
               </xsl:choose>
          </xsl:if>
     </xsl:template>
     <xsl:template name="findSpaceChar">
          <xsl:param name="str" />
          <xsl:variable name="strLength">
               <xsl:value-of select="string-length($str)" />
          </xsl:variable>
          <xsl:if test="$strLength &gt; 0">
               <xsl:choose>
                    <xsl:when test="substring($str, $strLength) != ' '">
                         <xsl:call-template name="findSpaceChar">
                              <xsl:with-param name="str" select="substring($str, 1, $strLength - 1)" />
                         </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                         <xsl:value-of select="$strLength" />
                    </xsl:otherwise>
               </xsl:choose>
          </xsl:if>
     </xsl:template>
     <xsl:template name="tempNguoiMua">
          <xsl:param name="str" />
          <xsl:variable name="strLength">
               <xsl:value-of select="string-length($str)" />
          </xsl:variable>
          <xsl:variable name="row1Length">
               <xsl:value-of select="85" />
          </xsl:variable>
          <xsl:choose>
               <xsl:when test="$strLength &gt; $row1Length">
                    <xsl:variable name="str0">
                         <xsl:value-of select="substring($str, 1, $row1Length)" />
                    </xsl:variable>
                    <xsl:variable name="spaceCharPosition">
                         <xsl:call-template name="findSpaceChar">
                              <xsl:with-param name="str" select="$str0" />
                         </xsl:call-template>
                    </xsl:variable>
                    <xsl:variable name="str1">
                         <xsl:value-of select="substring($str0, 1, $spaceCharPosition)" />
                    </xsl:variable>
                    <xsl:variable name="str2">
                         <xsl:value-of select="substring($str, $spaceCharPosition + 1)" />
                    </xsl:variable>
                    <div class="clsTable">
                         <div class="clsCol col-title">
                              <p style="font-family: 'Time new roman';">Họ tên người mua hàng:</p>
                         </div>
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                    <xsl:value-of select="$str1" />
                              </p>
                         </div>
                    </div>
                    <div class="clsTable">
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                   <xsl:value-of select="$str2" />
                              </p>
                         </div>
                    </div>
               </xsl:when>
               <xsl:otherwise>
                    <div class="clsTable">
                         <div class="clsCol col-title">
                              <p style="font-family: 'Time new roman';">Họ tên người mua hàng:</p>
                         </div>
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                    <xsl:value-of select="$str" />
                              </p>
                         </div>
                    </div>
               </xsl:otherwise>
          </xsl:choose>
     </xsl:template>
     <xsl:template name="tempTenKhachHang">
          <xsl:param name="str" />
          <xsl:variable name="strLength">
               <xsl:value-of select="string-length($str)" />
          </xsl:variable>
          <xsl:variable name="row1Length">
               <xsl:value-of select="75" />
          </xsl:variable>
          <xsl:choose>
               <xsl:when test="$strLength &gt; $row1Length">
                    <xsl:variable name="str0">
                         <xsl:value-of select="substring($str, 1, $row1Length)" />
                    </xsl:variable>
                    <xsl:variable name="spaceCharPosition">
                         <xsl:call-template name="findSpaceChar">
                              <xsl:with-param name="str" select="$str0" />
                         </xsl:call-template>
                    </xsl:variable>
                    <xsl:variable name="str1">
                         <xsl:value-of select="substring($str0, 1, $spaceCharPosition)" />
                    </xsl:variable>
                    <xsl:variable name="str2">
                         <xsl:value-of select="substring($str, $spaceCharPosition + 1)" />
                    </xsl:variable>
                    <div class="clsTable">
                         <div class="clsCol col-title">
                              <p style="font-family: 'Time new roman';">Tên đơn vị: </p>
                         </div>
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                    <xsl:value-of select="$str1" />
                              </p>
                         </div>
                    </div>
                    <div class="clsTable">
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                   <xsl:value-of select="$str2" />
                              </p>
                         </div>
                    </div>
               </xsl:when>
               <xsl:otherwise>
                    <div class="clsTable">
                         <div class="clsCol col-title">
                              <p style="font-family: 'Time new roman';">Tên đơn vị: </p>
                         </div>
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                    <xsl:value-of select="$str" />
                              </p>
                         </div>
                    </div>
               </xsl:otherwise>
          </xsl:choose>
     </xsl:template>
     <xsl:template name="tempThTien_words">
          <xsl:param name="str" />
          <xsl:variable name="strLength">
               <xsl:value-of select="string-length($str)" />
          </xsl:variable>
          <xsl:variable name="row1Length">
               <xsl:value-of select="75" />
          </xsl:variable>
          <xsl:choose>
               <xsl:when test="$strLength &gt; $row1Length">
                    <xsl:variable name="str0">
                         <xsl:value-of select="substring($str, 1, $row1Length)" />
                    </xsl:variable>
                    <xsl:variable name="spaceCharPosition">
                         <xsl:call-template name="findSpaceChar">
                              <xsl:with-param name="str" select="$str0" />
                         </xsl:call-template>
                    </xsl:variable>
                    <xsl:variable name="str1">
                         <xsl:value-of select="substring($str0, 1, $spaceCharPosition)" />
                    </xsl:variable>
                    <xsl:variable name="str2">
                         <xsl:value-of select="substring($str, $spaceCharPosition + 1)" />
                    </xsl:variable>
                    <div class="clsTable">
                         <div class="clsCol col-title">
                              <p style="font-family: 'Time new roman';">Số tiền viết bằng chữ: </p>
                         </div>
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                    <xsl:value-of select="$str1" />
                              </p>
                         </div>
                    </div>
                    <div class="clsTable">
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                   <xsl:value-of select="$str2" />
                              </p>
                         </div>
                    </div>
               </xsl:when>
               <xsl:otherwise>
                    <div class="clsTable">
                         <div class="clsCol col-title">
                              <p style="font-family: 'Time new roman';">Số tiền viết bằng chữ: </p>
                         </div>
                         <div class="clsCol col-txt">
                              <p class="input-txt" style="">
                                    <xsl:value-of select="$str" />
                              </p>
                         </div>
                    </div>
                    <!-- <div class="clsTable">         <div class="clsCol col-txt" style="width:100%;float:left">				   <p class="input-txt" style="">					    &#160;				   </p>				   </div>        </div>-->
               </xsl:otherwise>
          </xsl:choose>
     </xsl:template>
     <xsl:template name="addLine">
          <xsl:param name="count" />
          <xsl:if test="$count &gt; 0">
               <tr class="noline back" style="    border-top: #cc3333 dotted 1px ">
                    <td style="border-bottom:none" height="23px" width="24px">
                         <xsl:value-of select="''" />
                    </td>
                    <td style="border-bottom:none" height="23px" width="120px">
                         <xsl:value-of select="''" />
                    </td>
                    <td style="border-bottom:none" height="23px" width="41px">
                         <xsl:value-of select="''" />
                    </td>
                    <td style="border-bottom:none" height="23px" width="45px">
                         <xsl:value-of select="''" />
                    </td>
                    <td style="border-bottom:none" height="23px" width="79px">
                         <xsl:value-of select="''" />
                    </td>
                    <td style="border-bottom:none" height="23px" width="125px">
                         <xsl:value-of select="''" />
                    </td>
               </tr>
               <xsl:call-template name="addLine">
                    <xsl:with-param name="count" select="$count - 1" />
               </xsl:call-template>
          </xsl:if>
     </xsl:template>
     <xsl:template name="main">
          <xsl:param name="pagesNeededfnc" />
          <xsl:param name="itemCountfnc" />
          <xsl:param name="itemNeeded" />
          <xsl:for-each select="NDHDon//DSHHDVu//HHDVu">
               <xsl:choose>
                    <!-- Vị trí dòng product đầu mỗi trang -->
                    <xsl:when test=" position() mod $itemNeeded = 1">
                         <xsl:choose>
                              <!-- Dòng product đầu tiên của trang đầu -->
                              <xsl:when test="position()=1">
                                   <xsl:text disable-output-escaping="yes">&lt;div class="pagecurrent" id="1"&gt;</xsl:text>
                                   <xsl:call-template name="addfirtbody"></xsl:call-template>
                                   <xsl:call-template name="addsecondbody"></xsl:call-template>
                                   <xsl:text disable-output-escaping="yes">&lt;div class="statistics"&gt;</xsl:text>
                                   <xsl:text disable-output-escaping="yes">&lt;div class="nenhd"&gt;</xsl:text>
                                   <xsl:text disable-output-escaping="yes">&lt;table width="100%" class="dongcuoi" cellpadding="0" cellspacing="0" border="1" style="border-bottom: 0px solid"&gt;</xsl:text>
                                   <xsl:call-template name="calltitleproduct"></xsl:call-template>
                                   <xsl:call-template name="callbodyproduct"></xsl:call-template>
                                   <!-- Trường hợp chỉ có 1 sản phẩm product -->
                                   <xsl:if test="(position()=1) and $itemCountfnc=1">
                                        <xsl:call-template name="addLine">
                                             <xsl:with-param name="count" select="$pagesNeededfnc * $itemNeeded - $itemCountfnc" />
                                        </xsl:call-template>
                                        <xsl:call-template name="calltongsoproduct"></xsl:call-template>
                                        <xsl:text disable-output-escaping="yes">&lt;/table&gt;</xsl:text>
                                        <xsl:text disable-output-escaping="yes">&lt;div class="nenhd_bg" style=" "&gt;&lt;/div&gt;</xsl:text>
                                        <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                                        <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                                        <xsl:call-template name="addchuky"></xsl:call-template>
                                        <xsl:call-template name="addfinalbody"></xsl:call-template>
                                        <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                                   </xsl:if>
                              </xsl:when>
                              <!-- Dòng product đầu của các trang sau -->
                              <xsl:otherwise>
                                   <xsl:text disable-output-escaping="yes">&lt;div class="pagecurrent" id=</xsl:text>
                                   <xsl:value-of select="((position()-1) div $itemNeeded) + 1" />
                                   <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
                                   <xsl:call-template name="addfirtbody"></xsl:call-template>
                                   <div style="   text-align:center; margin-top: 3px;font-family: 'Time new roman'">
                                        <label style="    font-size: 16px;   margin-top: -22px;  float: right;">
                                             Tiep theo trang truoc - trang
                                             <xsl:value-of select="((position()-1) div $itemNeeded) + 1" />/
                                             <xsl:value-of select="$pagesNeededfnc" />
                                        </label>
                                   </div>
                                   <xsl:call-template name="addsecondbody"></xsl:call-template>
                                   <xsl:text disable-output-escaping="yes">&lt;div class="statistics"&gt;</xsl:text>
                                   <xsl:text disable-output-escaping="yes">&lt;div class="nenhd"&gt;</xsl:text>
                                   <xsl:text disable-output-escaping="yes">&lt;table width="100%" class="dongcuoi" cellpadding="0" cellspacing="0"  border="1" style="border-bottom: 0px solid"&gt;</xsl:text>
                                   <xsl:call-template name="calltitleproduct"></xsl:call-template>
                                   <xsl:call-template name="callbodyproduct"></xsl:call-template>
                                   <!-- Trường hợp dòng product cuối cùng là dòng đầu tiên của trang cuối cùng -->
                                   <xsl:if test=" position() = $itemCountfnc">
                                        <xsl:call-template name="addLine">
                                             <xsl:with-param name="count" select="$pagesNeededfnc * $itemNeeded - $itemCountfnc" />
                                        </xsl:call-template>
                                        <xsl:call-template name="calltongsoproduct"></xsl:call-template>
                                        <xsl:text disable-output-escaping="yes">&lt;/table&gt;</xsl:text>
                                        <xsl:text disable-output-escaping="yes">&lt;div class="nenhd_bg" style=" "&gt;&lt;/div&gt;</xsl:text>
                                        <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                                        <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                                        <xsl:call-template name="addchuky"></xsl:call-template>
                                        <xsl:call-template name="addfinalbody"></xsl:call-template>
                                        <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                                   </xsl:if>
                              </xsl:otherwise>
                         </xsl:choose>
                    </xsl:when>
                    <!-- Vị trí dòng product cuối cùng mỗi trang, không phải trang cuối -->
                    <xsl:when test=" (position() mod $itemNeeded = 0) and (position() &lt; $itemCountfnc)">
                         <xsl:call-template name="callbodyproduct"></xsl:call-template>
                         <xsl:text disable-output-escaping="yes">&lt;/table&gt;</xsl:text>
                         <xsl:text disable-output-escaping="yes">&lt;div class="nenhd_bg" style=" "&gt;&lt;/div&gt;</xsl:text>
                         <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                         <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                         <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                         <p style="page-break-before: always"></p>
                    </xsl:when>
                    <!-- Vị trí dòng sản phẩm cuối cùng -->
                    <xsl:when test=" position() = $itemCountfnc">
                         <xsl:call-template name="callbodyproduct"></xsl:call-template>
                         <xsl:call-template name="addLine">
                              <xsl:with-param name="count" select="$pagesNeededfnc * $itemNeeded - $itemCountfnc" />
                         </xsl:call-template>
                         <xsl:call-template name="calltongsoproduct"></xsl:call-template>
                         <xsl:text disable-output-escaping="yes">&lt;/table&gt;</xsl:text>
                         <xsl:text disable-output-escaping="yes">&lt;div class="nenhd_bg" style=" "&gt;&lt;/div&gt;</xsl:text>
                         <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                         <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                         <xsl:call-template name="addchuky"></xsl:call-template>
                         <xsl:call-template name="addfinalbody"></xsl:call-template>
                         <xsl:text disable-output-escaping="yes">&lt;/div&gt;</xsl:text>
                    </xsl:when>
                    <!-- Các vị trí dòng sản phẩm ở khoảng giữa một trang -->
                    <xsl:otherwise>
                         <xsl:call-template name="callbodyproduct"></xsl:call-template>
                    </xsl:otherwise>
               </xsl:choose>
          </xsl:for-each>
     </xsl:template>
     <xsl:template name="addfirtbody">
          <table width="100%" style="float: left; margin-top: 15px; border-bottom: solid 1px #cc3333;" class="comname">
               <!-- margin-left: 192px; -->
               <tbody>
                    <tr>
                         <td colspan="3" style="padding-left: 15px;">
                              <div class="clsTable">
                                   <div class="clsCol col-title">
                                        <p style="font-family: 'Time new roman';font-size:20px; text-transform:uppercase;font-weight:bold;">
                                             <xsl:value-of select="../../NBan/Ten" />
                                        </p>
                                   </div>
                              </div>
                              <div class="clsTable">
                                   <div class="clsCol col-title">
                                        <p style="font-family: 'Time new roman';">Mã số thuế: </p>
                                   </div>
                                   <div class="clsCol col-txt">
                                        <p class="" style="color: #cc3333 !important; letter-spacing:4; font-weight:bold;">
                                              <xsl:choose>
                                                  <xsl:when test="../../NBan//MST!=''">
                                                       <xsl:value-of select="../../NBan//MST" />
                                                  </xsl:when>
                                                  <xsl:otherwise>                                           </xsl:otherwise>
                                             </xsl:choose>
                                        </p>
                                   </div>
                              </div>
                              <div class="clsTable">
                                   <div class="clsCol col-title">
                                        <p style="font-family: 'Time new roman';">Địa chỉ: </p>
                                   </div>
                                   <div class="clsCol col-txt">
                                        <p class="" style="color: #cc3333 !important">
                                               <xsl:choose>
                                                  <xsl:when test="../../NBan//DChi!=''">
                                                       <xsl:value-of select="../../NBan//DChi" />
                                                  </xsl:when>
                                                  <xsl:otherwise>                                           </xsl:otherwise>
                                             </xsl:choose>
                                        </p>
                                   </div>
                              </div>
                              <div class="clsTable">
                                   <div class="clsCol col-title">
                                        <p style="font-family: 'Time new roman';   ">Điện thoại:</p>
                                   </div>
                                   <div class="clsCol col-txt">
                                        <p class="" style="color: #cc3333 !important">
                                              <xsl:choose>
                                                  <xsl:when test="../../NBan//SDThoai!=''">
                                                       <xsl:value-of select="../../NBan//SDThoai" />
                                                  </xsl:when>
                                                  <xsl:otherwise>                                           </xsl:otherwise>
                                             </xsl:choose>
                                        </p>
                                   </div>
                              </div>
                              <div class="clsTable">
                                   <div class="clsCol col-title">
                                        <p style="font-family: 'Time new roman';">Số tài khoản: </p>
                                   </div>
                                   <div class="clsCol col-txt">
                                        <p class="" style="color: #cc3333 !important">
                                              <xsl:choose>
                                                  <xsl:when test="../../NBan//TNHang!=''">
                                                       <xsl:value-of select="../../NBan//STKNHang" /> tại
                                                       <xsl:value-of select="../../NBan//TNHang" />
                                                  </xsl:when>
                                                  <xsl:otherwise>
                                                       <xsl:value-of select="../../NBan//STKNHang" />
                                                  </xsl:otherwise>
                                             </xsl:choose>
                                        </p>
                                   </div>
                              </div>
                         </td>
                    </tr>
               </tbody>
          </table>
          <table width="100%" style="">
               <tr>
                    <td style="width:26%;text-align: left;padding-left:15px;    vertical-align: top;padding-top: 7px;">
                         <div id="logo"> </div>
                    </td>
                    <td style="vertical-align: top; padding-top:10px;">
                         <table cellpadding="0" cellspacing="0" border="0" style="float: left; width:326px">
                              <tbody>
                                   <tr>
                                        <td style="text-align: center;padding-left: 0%;">
                                             <!--<p class="name-upcase" style="color:#EB363A; font-size:20px; text-transform: uppercase;"><xsl:value-of select="../../InvoiceName"/></p>-->
                                             <p class="name-upcase" style="    line-height: 25px;font-weight: bold;color:#cc3333; margin-top: 3px; margin-bottom: 0px;;font-size:24px; text-transform: uppercase;">
                                                  HÓA ĐƠN BÁN HÀNG
                                                  <br />
                                             </p>
                                             <p style="    font-size: 16px;margin-top: 0px;color:#cc3333;margin-top:10px;">
                                                  <i>
                                                       <xsl:choose>
                                                            <xsl:when test="substring(../../..//NLap,1,4)!= '1957' and substring(../../..//NLap,1,4)!= ''">
                                                                 Ngày
                                                                 <label style=" color:#000">
                                                                      <xsl:value-of select="substring(../../..//NLap,9,2)" />
                                                                 </label>                            tháng
                                                                 <label style=" color:#000">
                                                                      <xsl:value-of select="substring(../../..//NLap,6,2)" />
                                                                 </label>                            năm
                                                                 <label style=" color:#000">
                                                                      <!--<xsl:value-of select="substring(..//NLap,3,2)"/>-->
                                                                      <xsl:value-of select="substring(../../..//NLap,1,4)" />
                                                                 </label>
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                 Ngày
                                                                 <label style="border-bottom: 1px dotted #584C56;">    </label> tháng
                                                                 <label style="border-bottom: 1px dotted #584C56;">    </label> năm
                                                                 <label style="border-bottom: 1px dotted #584C56;">    </label>
                                                            </xsl:otherwise>
                                                       </xsl:choose>
                                                  </i>
                                             </p>
                                        </td>
                                   </tr>
                              </tbody>
                         </table>
                         <table border="0" style=" float: right;   margin-top: 2px;">
                              <tbody>
                                   <tr>
                                        <td class="header">
                                             <div class="header-note">
                                                  <p style="font-size:16px;margin-top: 5px;width: 100%;    color: #cc3333 !important;">
                                                       Ký hiệu:
                                                       <xsl:call-template name="formatSerial"></xsl:call-template>
                                                  </p>
                                                  <p style="margin-top:5px;font-size:16px;    color: #cc3333 !important;  ">
                                                       Số:
                                                       <b style="margin-top:0px;font-size:16px;">
                                                            <xsl:choose>
                                                                 <xsl:when test="../../../TTChung//SHDon!=''">
                                                                      <span class="number" style="color:#ff0000 !important;">
                                                                           <xsl:call-template name="addZero">
                                                                                <xsl:with-param name="count" select="8-string-length(../../../TTChung//SHDon)" />
                                                                           </xsl:call-template>
                                                                           <xsl:value-of select="../../../TTChung//SHDon" />
                                                                      </span>
                                                                 </xsl:when>
                                                            </xsl:choose>
                                                       </b>
                                                  </p>
                                             </div>
                                             <!--<p style="margin-top: 0px; font-size:15px;line-height: 19px;">                                         :                                       											<xsl:value-of select="../../InvoicePattern"/>																		<br/>                                      Ký hiệu:                               											<xsl:value-of select="../../../TTChung//KHHDon"/>																	<br/>										<span style="margin-top: 8px; margin-right:5px;float: left;">Số:</span>										<span class="number" style="color:#EB363A;font-size: 180%;line-height: 1;   font-family: 'Times New Roman' ,Times,serif; font-weight: bold;">											<xsl:call-template name="addZero">												<xsl:with-param name="count" select="7-string-length(../../../TTChung//SHDon)"/>											</xsl:call-template>											<xsl:value-of select="../../../TTChung//SHDon"/>										</span>									</p>-->
                                        </td>
                                   </tr>
                              </tbody>
                         </table>
                    </td>
               </tr>
               <tr>
                    <td colspan="2">
                         <xsl:choose>
                              <xsl:when test="//MCCQT != ''">
                                   <center style="font-style: italic;">
                                        <xsl:choose>
                                             <xsl:when test="//MCCQT !=''">
                                                  <p>
                                                       <b>Mã của cơ quan thuế:</b>
                                                       <b>
                                                            <xsl:value-of select="//MCCQT" />
                                                       </b>
                                                  </p>
                                             </xsl:when>
                                        </xsl:choose>
                                   </center>
                              </xsl:when>
                         </xsl:choose>
                         <xsl:choose>
                              <xsl:when test="../../../convert!=''">
                                   <p style="color:#000000; font-size: 16px; margin-top: 0px;    text-align: center;margin-bottom: 10px;">
                                        <i>( HÓA ĐƠN CHUYỂN ĐỔI TỪ HÓA ĐƠN ĐIỆN TỬ )</i>
                                   </p>
                              </xsl:when>
                              <xsl:otherwise>                           </xsl:otherwise>
                         </xsl:choose>
                    </td>
               </tr>
          </table>
     </xsl:template>
     <xsl:template name="addsecondbody">
          <table class="cusname" style="    border-top: #cc3333 solid thin;">
               <tr>
                    <td>
                         <xsl:call-template name="tempNguoiMua">
                              <xsl:with-param name="str" select="../../NMua/HVTNMHang" />
                         </xsl:call-template>
                         <xsl:call-template name="tempTenKhachHang">
                              <xsl:with-param name="str" select="../../NMua/Ten" />
                         </xsl:call-template>
                         <div class="clsTable">
                              <div class="clsCol col-title">
                                   <p style="font-family: 'Time new roman';">Mã số thuế:</p>
                              </div>
                              <div class="clsCol col-txt">
                                   <p class="input-txt" style="">
                                         <xsl:choose>
                                             <xsl:when test="../../NMua/MST!=''">
                                                  <xsl:value-of select="../../NMua/MST" />
                                             </xsl:when>
                                             <xsl:otherwise>                                       </xsl:otherwise>
                                        </xsl:choose>
                                   </p>
                              </div>
                         </div>
                         <div class="clsTable">
                              <div class="clsCol col-title">
                                   <p style="font-family: 'Time new roman';">Địa chỉ: </p>
                              </div>
                              <div class="clsCol col-txt">
                                   <p class="input-txt" style="">
                                         <xsl:choose>
                                             <xsl:when test="../../NMua//DChi!=''">
                                                  <xsl:value-of select="../../NMua//DChi" />
                                             </xsl:when>
                                             <xsl:otherwise>                                       </xsl:otherwise>
                                        </xsl:choose>
                                   </p>
                              </div>
                         </div>
                         <div class="clsTable">
                              <div class="clsCol col-title">
                                   <p style="font-family: 'Time new roman';">Số tài khoản:</p>
                              </div>
                              <div class="clsCol col-txt">
                                   <p class="input-txt" style="">
                                         <xsl:choose>
                                             <xsl:when test="../../NMua//STKNHang!=''">
                                                  <xsl:value-of select="../../NMua//STKNHang" />
                                             </xsl:when>
                                             <xsl:otherwise>                                       </xsl:otherwise>
                                        </xsl:choose>
                                   </p>
                              </div>
                         </div>
                         <div class="clsTable">
                              <div class="clsCol col-title">
                                   <p style="font-family: 'Time new roman';">Hình thức thanh toán:</p>
                              </div>
                              <div class="clsCol col-txt" style="width:40%">
                                   <p class="input-txt" style="">
                                         <xsl:choose>
                                             <xsl:when test="../../../TTChung//HTTToan!=''">
                                                  <xsl:value-of select="../../../TTChung//HTTToan" />
                                             </xsl:when>
                                             <xsl:otherwise>                                       </xsl:otherwise>
                                        </xsl:choose>
                                   </p>
                              </div>
                         </div>
                    </td>
               </tr>
          </table>
     </xsl:template>
     <xsl:template name="calltitleproduct">
          <tr style="border-bottom: #cc3333 1px solid" height="20px">
               <th width="25px" class="h1">        STT      </th>
               <th width="205px" class="h1">        Tên hàng hóa, dịch vụ      </th>
               <th width="50px" class="h1">        Đơn vị tính      </th>
               <th width="45px" class="h1">        Số lượng      </th>
               <th width="39px" class="h1">        Đơn giá      </th>
               <th width="60px" class="h1">        Thành tiền      </th>
          </tr>
          <tr style=" height:20px;color:#cc3333;">
               <th class="h2">        1      </th>
               <th class="h2">        2      </th>
               <th class="h2">        3      </th>
               <th class="h2">        4      </th>
               <th class="h2">        5      </th>
               <th class="h2">        6=4x5      </th>
          </tr>
     </xsl:template>
     <xsl:template name="callbodyproduct">
          <xsl:choose>
               <xsl:when test="Extra2=0">
                    <tr class="noline back" style="    border-bottom: #cc3333 dotted 1px ">
                         <td class="stt" height="23px" style="text-align: left;">
                              <xsl:value-of select="position()" />
                         </td>
                         <td class="back-bold2" height="23px">
                              <strong>
                                   <xsl:value-of select="THHDVu" />
                              </strong>
                         </td>
                         <td class="back-bold2" height="23px" style="text-align:center">
                              <xsl:value-of select="DVTinh" />
                         </td>
                         <td class="back-bold" height="23px" style="text-align:center">
                              <xsl:choose>
                                   <xsl:when test="SLuong=''">
                                        <xsl:value-of select="''" />
                                   </xsl:when>
                                   <xsl:when test="SLuong=0"></xsl:when>
                                   <xsl:otherwise>
                                        <!-- <xsl:value-of select="translate(translate(translate(format-number(SLuong, '###,###'),',','?'),'.',','),'?','.')"/> -->
                                        <xsl:value-of select="translate(translate(translate(format-number(SLuong, '###,##0.##'),',','?'),'.',','),'?','.')" />
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                         <td class="back-bold" height="23px">
                              <xsl:choose>
                                   <xsl:when test="DGia=''">
                                        <xsl:value-of select="''" />
                                   </xsl:when>
                                   <xsl:when test="DGia=0">                               </xsl:when>
                                   <xsl:otherwise>
                                        <!-- <xsl:value-of select="translate(translate(translate(format-number(DGia, '###,###'),',','?'),'.',','),'?','.')"/> -->
                                        <xsl:value-of select="translate(translate(translate(format-number(DGia, '###,##0.##'),',','?'),'.',','),'?','.')" />
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                         <td class="back-bold" height="23px">
                              <xsl:choose>
                                   <xsl:when test="(ThTien=0) or(ThTien='')">
                                        <xsl:value-of select="ThTien" />
                                   </xsl:when>
                                   <xsl:otherwise>
                                        <xsl:value-of select="translate(translate(translate(format-number(ThTien, '###,###'),',','?'),'.',','),'?','.')" />
                                        <!-- <xsl:value-of select="format-number(ThTien, '###,###.###')" /> -->
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                    </tr>
               </xsl:when>
               <xsl:otherwise>
                    <tr class="noline back" style="border-bottom: #cc3333 dotted 1px ">
                         <td class="stt" height="23px">
                              <xsl:choose>
                                   <xsl:when test="(TChat=4)">
                                        <xsl:value-of select="''" />
                                   </xsl:when>
                                   <xsl:otherwise>
                                        <xsl:value-of select="STT" />
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                         <td class="back-bold2" height="23px">
                              <xsl:value-of select="THHDVu" />
                         </td>
                         <td class="back-bold2" height="23px" style="text-align:center">
                              <xsl:value-of select="DVTinh" />
                         </td>
                         <td class="back-bold" height="23px" style="text-align:center">
                              <xsl:choose>
                                   <xsl:when test="SLuong=''">
                                        <xsl:value-of select="''" />
                                   </xsl:when>
                                   <xsl:when test="SLuong=0">
                                        <!-- <xsl:value-of select="SLuong"></xsl:value-of> -->
                                   </xsl:when>
                                   <xsl:otherwise>
                                        <xsl:value-of select="translate(translate(translate(format-number(SLuong, '###,##0.##'),',','?'),'.',','),'?','.')" />
                                        <!-- <xsl:value-of select="translate(translate(translate(format-number(SLuong, '###,###'),',','?'),'.',','),'?','.')"/> -->
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                         <td class="back-bold" height="23px">
                              <xsl:choose>
                                   <xsl:when test="DGia=0">                               </xsl:when>
                                   <xsl:when test="DGia=''">
                                        <xsl:value-of select="''" />
                                   </xsl:when>
                                   <xsl:otherwise>
                                        <xsl:value-of select="translate(translate(translate(format-number(DGia, '###,##0.##'),',','?'),'.',','),'?','.')" />
                                        <!-- <xsl:value-of select="translate(translate(translate(format-number(DGia, '###,###'),',','?'),'.',','),'?','.')"/> -->
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                         <td class="back-bold" height="23px">
                              <xsl:choose>
                                   <xsl:when test="(TChat=4)">
                                        <xsl:value-of select="''" />
                                   </xsl:when>
                                   <xsl:when test="ThTien=''">
                                         
                                   </xsl:when>
                                   <xsl:when test="ThTien=0"></xsl:when>
                                   <xsl:otherwise>
                                        <xsl:value-of select="translate(translate(translate(format-number(translate(ThTien,',','.'), '###,##0'),',','?'),'.',','),'?','.')" />
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                    </tr>
               </xsl:otherwise>
          </xsl:choose>
     </xsl:template>
     <xsl:template name="calltongsoproduct">
          <tfoot>
               <tr class="noline back" style="border-top: 1px solid #cc3333; border-bottom: 0px;">
                    <td class="back-bold" colspan="5" height="23px" width="136px" style="padding-right: 20px;border-right:none ; text-align:left;   color: #cc3333 !important;    padding-left: 296px;">          Cộng tiền hàng hóa, dịch vụ:        </td>
                    <td colspan="1" class="back-bold" height="23px" style="border-left:none;text-align:right">
                         <b style="width:100%;  float:right;font-weight:normal;       border-bottom: 1px dotted rgba(0, 0, 0, 0.5);     line-height: 15px; ">
                              <xsl:choose>
                                   <xsl:when test="//TToan/TgTTTBSo=''">                                </xsl:when>
                                   <xsl:when test="//TToan/TgTTTBSo=0">
                                        <xsl:value-of select="../../TToan/TgTTTBSo" />
                                   </xsl:when>
                                   <xsl:otherwise>
                                        <!-- <xsl:value-of select="translate(translate(translate(format-number(../../TToan/TgTTTBSo, '###,###'),',','?'),'.',','),'?','.')"/> -->
                                        <xsl:value-of select="translate(translate(translate(format-number(//TToan/TgTTTBSo, '###,###'),',','?'),'.',','),'?','.')" />
                                   </xsl:otherwise>
                              </xsl:choose>
                         </b>
                    </td>
               </tr>
               <xsl:if test="../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu!='0,00'">
                    <tr style="border: 1px solid #cc3333 !important;">
                         <td colspan="6" style=" padding-left: 10px;">
                              <xsl:choose>
                                   <xsl:when test="not(../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu)"></xsl:when>
                                   <xsl:when test="../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu = ''"></xsl:when>
                                   <xsl:when test="../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu = '0,00'"></xsl:when>
                                   <xsl:when test="../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu = '0'"></xsl:when>
                                   <xsl:otherwise>
                                        Đã giảm <xsl:choose>
                                             <xsl:when test="../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu!=''">
                                                  <!--<xsl:value-of select="../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu" />-->
                                                  <xsl:value-of select="translate(translate(translate(format-number(translate(../../TToan/TTKhac/TTin[TTruong='Extra10']/DLieu,',','.'), '###,##0'),',','?'),'.',','),'?','.')" />
                                             </xsl:when>
                                             <xsl:otherwise>                                   </xsl:otherwise>
                                        </xsl:choose> đồng tương ứng 20% mức tỷ lệ <xsl:choose>
                                             <xsl:when test="../../TToan/TTKhac/TTin[TTruong='Extra9']/DLieu!=''">
                                                  <xsl:value-of select="../../TToan/TTKhac/TTin[TTruong='Extra9']/DLieu" />
                                             </xsl:when>
                                             <xsl:otherwise></xsl:otherwise>
                                        </xsl:choose><xsl:choose>
                                             <xsl:when test="substring(../../..//NLap,1,4)!= '1957' and substring(../../..//NLap,1,4)!= ''">
                                                  <xsl:choose>
                                                       <xsl:when test="substring(../../..//NLap,1,4) &gt; 2024">
                                                            % để tính thuế giá trị gia tăng theo Nghị quyết số 110/2023/QH15
                                                       </xsl:when>
                                                       <xsl:when test="substring(../../..//NLap,1,4) &gt; 2023 or (substring(../../..//NLap,1,4) = 2023 and substring(../../..//NLap,6,2) &gt;= 7)">
                                                            % để tính thuế giá trị gia tăng theo Nghị quyết số 142/2024/QH15
                                                       </xsl:when>
                                                       <xsl:otherwise> % để tính thuế giá trị gia tăng theo Nghị quyết số 43/2022/QH15</xsl:otherwise>
                                                  </xsl:choose>
                                             </xsl:when>
                                             <xsl:otherwise>
                                                  % để tính thuế giá trị gia tăng theo Nghị quyết số 142/2024/QH15
                                             </xsl:otherwise>
                                        </xsl:choose>
                                   </xsl:otherwise>
                              </xsl:choose>
                         </td>
                    </tr>
               </xsl:if>
               <xsl:if test="../../DSHHDVu/HHDVu[TChat = '4' and MHHDVu ='GHICHUNQ43']/THHDVu">
                    <tr style="border: 1px solid #cc3333 !important;">
                         <td colspan="6" style=" padding-left: 10px;">
                              <xsl:value-of select="../../DSHHDVu/HHDVu[TChat = '4' and MHHDVu ='GHICHUNQ43']/THHDVu" />
                         </td>
                    </tr>
               </xsl:if>
               <!--<tr class="noline back" style="border-top: #cc3333 solid thin; border-bottom:0px">				<td class="back-bold" colspan="6" height="20px" width="136px" style="border-right:none;text-align:left;    padding-left: 10px;">								 <div class="clsTable">														<div class="clsCol col-title"><p style="font-family: 'Time new roman';"> Thuế suất GTGT:&#160;                                                    </p></div>													   <div class="clsCol col-txt" style="width:20%">													   <p class="input-txt" style="text-align:left" >													  <xsl:choose>														<xsl:when test="../../TToan/THTTLTSuat/LTSuat/TSuat =-1">																/														</xsl:when>														<xsl:when test="../../TToan/THTTLTSuat/LTSuat/TSuat!=''">																<xsl:value-of select="../../TToan/THTTLTSuat/LTSuat/TSuat"/>%														</xsl:when>														<xsl:otherwise>																<xsl:value-of select="''"/>%														</xsl:otherwise>													</xsl:choose>													   </p>													   </div>													   <div class="clsCol col-title"><p style="font-family: 'Time new roman';padding-left: 20px;"> Tiền thuế GTGT:&#160;                                                    </p></div>                                                     <div class="clsCol col-txt">													   <p class="input-txt" style="text-align:right" >													   <xsl:choose>													<xsl:when test="../../TToan/THTTLTSuat/LTSuat/TSuat =-1">																											/																													</xsl:when>												<xsl:when test="../../TToan/THTTLTSuat/LTSuat/TThue=''">																											&#160;&#160;																											  </xsl:when>												<xsl:when test="../../TToan/THTTLTSuat/LTSuat/TThue=0">																												0																											  </xsl:when>												<xsl:otherwise>													-->
               <!-- <xsl:value-of select="translate(translate(translate(format-number(../../TToan/TgTTTBSo, '###,###'),',','?'),'.',','),'?','.')"/> -->
               <!--													<xsl:value-of select="translate(translate(translate(format-number(../../TToan/THTTLTSuat/LTSuat/TThue, '###,###'),',','?'),'.',','),'?','.')"/>												</xsl:otherwise>											</xsl:choose>													   </p>													   </div>				</div>								</td>			</tr>			<tr class="noline back" style="border-top: #cc3333 solid thin; border-bottom: 0px; ">					<td class="back-bold" colspan="6" height="20px" width="136px" style="border-right:none;     padding-left: 296px;  border-left:none;text-align:left; color: #cc3333 !important;">					<div id="bachmai"/>					    <div class="clsTable">					<div class="clsCol col-title"><p style="font-family: 'Time new roman';">Tổng cộng tiền thanh toán:</p></div>                   <div class="clsCol col-txt">				   <p class="input-txt" style="text-align:right">				   <xsl:choose>							<xsl:when test="../../TToan/TgTTTBSo=''">	                    																	&#160;&#160;	                    																  </xsl:when>							<xsl:when test="../../TToan/TgTTTBSo=0">								<xsl:value-of select="../../TToan/TgTTTBSo"/>							</xsl:when>							<xsl:otherwise>								<xsl:value-of select="translate(translate(translate(format-number(../../TToan/TgTTTBSo, '###,###'),',','?'),'.',','),'?','.')"/>								-->
               <!-- <xsl:value-of select="translate(translate(translate(format-number(../../TToan/TgTTTBSo, '###,###'),',','?'),'.',','),'?','.')"/> -->
               <!--							</xsl:otherwise>						</xsl:choose>				   </p>				   </div>                  </div>									-->
               <!--Phan trang-->
               <!-- 				</td>			</tr>-->
               <tr class="noline back" style="border-top: #cc3333 solid thin; border-bottom: #cc3333 solid thin">
                    <td class="back-bold" colspan="6" height="23px" width="136px" style="   padding-left: 10px;    text-align: left;&#xD;&#xA;">
                         <xsl:call-template name="tempThTien_words">
                              <xsl:with-param name="str" select="../../TToan/TgTTTBChu" />
                         </xsl:call-template>
                    </td>
               </tr>
          </tfoot>
     </xsl:template>
     <xsl:template name="addfinalbody">
          <div class="statistics"></div>
     </xsl:template>
     <xsl:template name="addchuky">
          <div class="statistics">
               <table width="790px" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                         <td style="padding-bottom: 0;">
                              <table>
                                   <tbody>
                                        <!--panel footer-->
                                        <!--variable-->
                                        <xsl:variable name="serial">
                                             <xsl:value-of select="../../../TTChung//KHHDon" />
                                        </xsl:variable>
                                        <xsl:variable name="pattern"></xsl:variable>
                                        <xsl:variable name="invno">
                                             <xsl:value-of select="../../../TTChung//SHDon" />
                                        </xsl:variable>
                                        <!---->
                                        <!--panel adjust-->
                                        <xsl:choose>
                                             <xsl:when test="../../isAdjust">
                                                  <tr>
                                                       <td>
                                                            <div id="AdjustInv" style="text-align:center;padding-top:0px;font-size: 16px;text-transform:uppercase">
                                                                 <xsl:value-of select="../../isAdjust" />
                                                            </div>
                                                       </td>
                                                  </tr>
                                             </xsl:when>
                                             <xsl:otherwise>
                                                  <xsl:choose>
                                                       <xsl:when test="../../../TTChung/TTHDLQuan/GChu">
                                                            <tr>
                                                                 <td>
                                                                      <div id="ReplaceInv" style="text-align:center;padding-top:0px;font-size: 16px;text-transform:uppercase">
                                                                           <xsl:value-of select="../../../TTChung/TTHDLQuan/GChu" />
                                                                      </div>
                                                                 </td>
                                                            </tr>
                                                       </xsl:when>
                                                       <!--	<xsl:otherwise>														<div style="text-align:center;padding-top:0px;font-size:15px;text-transform:uppercase">															<xsl:value-of select="'&#160;'"/>														</div>													</xsl:otherwise>-->
                                                  </xsl:choose>
                                             </xsl:otherwise>
                                        </xsl:choose>
                                        <tr>
                                             <td style="padding-bottom: 10px;">
                                                  <xsl:call-template name="addfinalbodyTT78" />
                                             </td>
                                        </tr>
                                   </tbody>
                              </table>
                         </td>
                    </tr>
               </table>
          </div>
          <!-- <xsl:choose>		<xsl:when test="../../../convert!=''">	                <div class="clearfix">					    <label class="fl-l">Tra cứu hóa đơn chuyển đổi tại website:</label>						<label class="fl-l input-name" style="width:277px; height:15px"><xsl:value-of select="''"/></label>					</div>		</xsl:when>	</xsl:choose> -->
     </xsl:template>
     <xsl:template match="/">
          <html xmlns="http://www.w3.org/1999/xhtml">
               <head>
                    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
                    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
                    <!--<link href="styles.css" type="text/css" rel="stylesheet" />-->
                    <title>VAT</title>
                    <div style="display:none"><![CDATA[if (lt IE 9)<script src="http://ie7-js.googlecode.com/svn/version/2.1(beta4)/IE9.js"></script>]]></div>
                    <style type="text/css" rel="stylesheet">
                         @charset utf-8;          * html,          body {          margin: 0;          padding: 0;          font-family: 'Time new roman';          font-size: 15px;          background-color: rgba(255, 255, 255, 0);          }          #main {          margin: 0 auto          }          .VATTEMP {          background-color: #fff;
                         <!--width:769px !important;-->          font-size:17px;          overflow: hidden;          border: 0px solid;          font-family: 'Times New Roman' !important;          }          .VATTEMP .header-main,          .content {          width: 790px          }          .VATTEMP .header {          float: left;          width:100%;          }          .VATTEMP .header-content {          float: left;          text-align: center;          width: 400px          }          .VATTEMP .comname td b{          font-size:15px;          }          .VATTEMP .header h2 {          font-size: 1em          }          .VATTEMP .header h2,          .header p {          margin: 0          }          .VATTEMP .header p.name-upcase {          font-size: 15px;          text-transform: uppercase          }          .VATTEMP .header-note {          float: right;          font-size: 16px;          padding-right: 10px;          }          .VATTEMP .header .number {          font-family: 'Time new roman';          font-size: 21px;          margin-left: 3px;          }          #logo{          background-image:url('');          background-repeat: no-repeat;          background-size: 100%;          background-position: center center;          width: 80px;          margin: auto;          height: 80px;          position: absolute;          }          .clearfix:after {          clear: both;          content: ".";          display: block;          height: 1px;          overflow: hidden;          visibility: hidden          }          .clearfix {          clear: both          }          .VATTEMP .cusname{          width:100%;          }          .VATTEMP .cusname td{          padding: 0px 15px;          }          .VATTEMP .input-code {          border: 1px solid #000;          color: #000;          float: left;          font-weight: normal;          text-align: center;          width: 18px;          height: 15px          }          .VATTEMP .dongcuoi tr:last-child{          border-bottom:#cc3333 solid thin !important;          }          .VATTEMP div label.fl-l,          div label {          margin-right: 5px;          margin-top: 0px          }          .VATTEMP .input-name,          .input-date {          border: 0;          border-bottom: 1px dotted #000          }          .VATTEMP .statistics {          clear: both;          margin-right: 0;          padding-top: 2px          }          .nenhd {          position: relative          }          .nenhd_bg {          background-image: url();          background-repeat: no-repeat;          opacity: 0.6;          width: 100%;          height: 412px;          top: 20px;          left: 0;          /* right: 0; */          margin: 0 auto;          background-size: 51%;          position: absolute;          background-position: center center;          text-align: center;          vertical-align: middle;          z-index: 1;          }          .VATTEMP .pagecurrent{          border:#cc3333 solid 0;          border-bottom: 0px !important;          padding:0 10px;          background:url(data:image/png;base64,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lfTm1r23nnvh034hezBx9vavOpXi5vDZi/vdTpP9g91zlYWjT07e98Dv3PY/bx28HW1/+dHyyfbFFz9545WXP3H67rtPukKQe+C+Spomfnpj895XvvP0//XUHXd8KXPiyLnESvzdmzde6O01SZudx2onj3+z/K57Pn/6f3pzd9H/kxc0H82fbT/7g8+Nmgcn548fexjP445nq+K5X1vI7z138ZPucxc/Xf+puz/v33Xmy73t/X7nwlXs3f6X4yjJ1d93339/97fityVfXv4vF/Lq8soH1l5++RN43qBULn+uOD394bv+Ujx+9ZdyZ68/9fQ/kkpz4l33fp7FxXPjVy7Hrf099ja3HstNTlyavPuuL9z9R2/+Lvp/0iD8N2qLySsrH+pduvpxZ656UZSz/6Bw9vQxWa6s9V++/JH9V1c+VM7kd2pnTn5NNAo7uy+9uOHvtrh+Y2Vl7v53//PC7ae/cseX3toJ7j8Y95/3Hti/cvHT69cuf/T2s2dmta0pPfjBDKPUji9d/+DWpVceXTx25Jw4fezxeNSda9+88dRor/mleBTmC3ee/WLttpNfu/1337qFy3/MhZ+VD7cvXvpY/+Dg5OKZ2x4e5x0e/H5JfPdv5s+KCy99vL25de+xu+/5vHfsxNdHtzb741sbbG/tfDly4ei9Z79QPrZ07ujvrP/E++nVv11b7F27+sitly59PFutXSnUq58rLs6SO7KQCa9e/+DWs89/RgyCUv6DH/i/VWZmLnReeGU8Xt+it751rjA9fbF+37v/+ek/fms3df6Dc9Lfmi1vvnDx08Prqw9NN+qvpgXvHzfedXZOZLPN9MrKw68+9cx/uTA3/0zx7jN/kBSzTX3h/H5na4PN/uDC3B1nvpw7fford/ybn3zbL/36UTvdvPXgxovPfVbvN5dna7UHZWOSO38wKW58or44unLj4RvXrjyycPrEY5ljS+eSYFwPr974q97Nza9upXHuyLve9Tv1205+/fjv3kh+4vnyd5b88OrKwxvPXvicrTXVU8sfUdUS73rCEZf+8/zZ4YULn93vtpePnL3rC+7C4nfSqze2xwdNVtdXz+lStrl056k/zS6f+trJ/+dbV4j+B8eXX60sB6+sPdR8+uIn/VJls7I892lOzN2Vma5diVZuPdh88tnPqVLuwHv/PZ+vF2pXhhde6iftAZsrN85V5+efrj543++e/oPm2tuR6698or649/KlR8evvPKx4p0nH3Qcl/rp09Mq63f0i9d+4fq3vv8P6u8686XCyeUnXOkMRs9d3Njt937noNlcXrzvri8Ulo8+efp/fmsXuv/BuwQfy5/tvvDSx4drGw/MHz36cFzOU733bkf1erPdy688+srlyx85e/bsl/LHT30t7fS221ev0W62P++EsV+5/57PF04sPn3sf1p9W8b1F9/Hh7qvvPqRIArKlcWZT+p6mXd9yxUrj9oPXXvmB59LlLKP3Xb7V9z5hafHN29uDPZ2Wbl580K+UV1ZuOPsF+567O2p11/91HR9/9VLH7/64nOfXVqYPZ9x/b9Xvf2Ou059ZXDx5Z91H2xv759+76XS737rQ86D6uKFp6bCkIzvg+cT2DYjT5AITVanBP/6zxEvfvwX9R0f/jBKQCodemmCnXeBhDRN6PfTz1eW7/nSXU9FTzzzcOGB+5/on7/0d47Ye89+7x+Mh4PJ2pGjTz7wzbcnGD92h+PvHrW3rl39hZ2NjQeqjcalmdtu/+rt/3IteOGTU1PxYFh719f6l64+Ujzdv3rlkaZI/Npdp79y75ffnoLuxxYzf2euvPH8c5+N2+3F/Oz8+fd9N/slgG9/sHC2UCjs3vuVrZ3zPy8f7r56+SNBPts89sD9v3P7v9o+eCe0/YXPztR7P3jh09nmsJY5ufzk6W+pJwC++37rodr01MXb/+1m58mfiT7WbB6cqlSr14/cdttX38rd3P9Ng9gv2g91rl//INlss3L7nV+6+385fLfiu+/uffa9f1X8/KW/u+ivvvDyx2UrKldmli68+9v9p3mH+O7D8uGDzc17q/Xq9Yee8r4K8MwvZu7VQVh+4C/VExd+KXtva2Xl4WGi7CN33/37Z9+mBcD/mnNn938j6vdm80tHn/ypJw/v2j714dLpQrG4efbL652nHg4+PtjdvLcic83q3fd8/vjvb70jcv2VT0/M7j/1g88VktQf3nXnl977tcMdz++/Vz7cWF48d+zf3Ey+/d7Bp/e3d85OTE1dfOip/BfeKTF/4RfLJ/euX30k8K1g+q4zX37Xv9k+eOUzU1N76+v3v/9J52svf3Y+f/DCyx8fD8Py9OnbvnLXV9+eouh/zffek3xEHHSW3JzfKd5zx5eO/8ubydMfEQ+nQtvv+1Mef/oTxdPdm+sPJMOgvHD27i/c9aV3Rr4AnLuj99lQxr43VV156PHDXP/2e8KPNWbnz5/+g73Nxx8cf7yws3eaQmF38l13fWH5X749Gzs/Vmx8srbY+u7539ClwqY8fuKxn3rt/dDvvF898r5vy8cAvvFA+3NWNyiX6tMX73+Kx98pMb/wi4XTV5+/8LnK7NTFmdO3ffXOf73ZufiJwulRtz/3U9/g8Vc/OTF78NLlj/YGw8mZu05/+a4/7l96p7T9++9KPhb223NROXvwwb/KfQnghZ/P3ps6VnDv1/qXnv6lw6ch7HHiLzzwwO8c+59vvCPm0su/0pi9un3tEa8zLjeqE5fu/c5hXfjCu51HnOWJp09/cbPzg4esh5Lt7XuDcnHjZ5558x8Z+9/rykfLJw+ef/ZzanLqYnlu/uk7/ri9culvz9R3bt5838PnvK9e/jtH7I1nnv0cYeoXjyyde+Abb3wf8W3Nl59zH9y8duUX6tNTFxu3nfz66c/fGpz/m+WT0V7z2Hu/aT328ieqy3svX3o0sqykfur4Y/d9uXvlndL2bz/U+2xz9daDU3OL5x58uvAFgBf/5gn7zv/lavLMQ9mHoisvfKxip7/hew6Jhn6Q4ns2jpQ4KuHg3z2GeOljP6+Pfuzn6B+fxirXIZfDzroUywVAE0eOc9s//fGC8+lf8h948E/e2tut/7/45i9l7600Ji6961++cZV86VNzZT3sT575k3fOhXzDQPC52fLWjc0HHv7LH58QfvA3yiczBXdw5l+/c4rRNxR4/1nx9CCJ/fv/YvxjHfy7n6guv/cP3p5d0f+kBcGj2Xstp7T20//uxxeGz/xSZfn+P2m/Y9v+jUe8Bz782I+/j3Phb09NxVGUe/eX3plxf/VXF/JrW7unP/QXP9727/2N7L35an7nzt99h+b6I7MnNzNJ9+f+6Md3EL/7sdzZ9351ePGdmi9PPJI7W6zVVt71hR8vlr//seryT331ndtPv//+7IM/9e3Rj20kXPgbjVntWel9X9zZeSe2+4W/vehv7G/d+4uP/fgd3KcfLZ988CudK+/UmP/V35orh0GYe+gPf/zdkG/9cu7sz/zxOzfXv/1B98FKbeLS2X/743dXLnxsaurer74z8wXg3If9Bx76xo/XWM/+57NlaQnu+eJP/o7Rf9IC8lPT9e5Ba/kDj//4uP5Xv1xffPcfH6y9U2P+vV+uLCcq8d/3pz++qP3uL5dPvveP37n99NzP+w9UJmYu3Pn7P76ofeaXJmfv/5Pdd+Zc+msL+Z21Ww9+4Bs/XvO+8n9Z8m2fcSAixmHIaL+H0w/xewOy69v0f+/PES8++vP6tp/5aeLZCazKDNLKoDXE7X2woBWrf/i+Fyf/GYZhGMY7xo3ffNgEwTAM4/+gjv6TJ0wQ/iOeuPfgN0q2/m07ATdbxLE8lIpJO9s4zSatP/oWtlIK2/HYffZFkuFFwlDiWC4y61ObaXx+5s47L4I20TQMwzAMwzAM4y23WJ+62Hnuhc8P1nY/6+32ka5D4GvGNZg/MgNCYLvZHAQBtZzP2HdxatM4k5MkUmEXMueOfVebpaJhGIZhGIZhGD8Rxx5Pzr3w4NzJ0uz0Z3O9lHh7G2vUJvZGKJkiADsOQwY3byDmJvGPHCPJZxlUM4S2ojozc+HiSWt5dW3/dGli6Tvv/7fvzGcwDcMwDMMwDMP4P7aXPj5X3tlde7CyXFlV+dK5jZsr1HQGvzGBO6pQWLvB4OUbJHGMrcdjkuNLZO48g9eVxIOYfriP6we46xsvq56i4tb/4fueeONf2zUMwzAMwzAMw3iz3PHljc637uos2t/b+/OMJ1jUQ/oiou9JCuUKlepP0XxpheDpq9huLkd5aYl4PGLci2j3R7gVl5yQhCqh3Wk94d5x6gqX/+MnNS+ZGoZhGIZhGMb/d///UDf/7/3yArc+ean58qV/vlAr/EbOdRiN+oyCiCRrE9iS2vIiVrWKVKlGdXrsrq0yVkNKJZ/cKCSjXPo7+/Qzfvfdr30nvGEYhmEYhmEYxlvlwb9Mzo1U6je3tkE45BSUlET1exzcuoEadVFKIeNoTLSyRtDv0+7tg4zxMzlUs09zfZ/5d73rd0w4DcMwDMMwDMP4SVh+17t+p7XfuhAdtMj6ebKuT9xpE/W77K+8yqDfRwohdNIfUS2WmDuyQK6UI9rYpHflBpXpI1/2F5bOmVAahmEYhmEYhvGTcPtjw4uZxuSl3ZWbDHb38XMlGlPT1BpVhsGANE2RQkphV+uUZ48gbYuDG1eJem3I5pHTc+eP//bNxITSMAzDMAzDMIyflNzi3HmdyxENxnRfXcHJlSlNTFKoVZFSIB0/g798nBiP9uYusVL4R2fpZ5wn8seOPW5CaBiGYRiGYRjGT5J37OgTfds6Z1drBLZHa22dsRI0jt9GPpdHKgG6WKYXpOQzFaaPnkBNVhnOT1w68ye9SyaEhmEYhmEYhmH8JJ39o84Vd2bqojc9xdTpM8hCmSiV4ObAcZCebTNAYpUbZPwiIDgIh7+bO738hAmfYRiGYRiGYRhvh9k7z3y5NR7+VqzBr01g5SoMpYN0HORw0Cdu7RPHAaJQIhT2f5VbWH78oT82f9zSMAzDMAzDMIy3x5mvRk/nlo5/fZDyGdwMqVKoTg8Vhsi4nKOaUxSSfXp2+qn2zPIT93zN+6oJm2EYhmEYhmEYb6e7/jw9N2jMnw+l84t2MCQfB0gB0rIscDyEl0G5fven/3h0wYTLMAzDMAzDMIx3goe+1rkiM+5A+x6qmEOlChmPx6S9Hu3+CDtf3DBhMgzDMAzDMAzjnUR57qA/HjLqt0mTBGkJwTgIySwsosI4//yv1BZNmAzDMAzDMAzDeCd4+W9N1+Mk9rOz0wRJhEYjLdclOzdPgsB23Kcspe0ffGJyyoTLMAzDMAzDMIy30/OfnC3rcVjyhHhKZn0yUxNIaSGl4xJ6LhnbQaea0fbe9e7O7lkTMsMwDMMwDMMw3k7RYDDZ29q+7jgucRjglotYrotUaYxMEzytcFyHfrtF0ZKJCZlhGIZhGIZhGG+nZByUo4MOnrTwhMV4HKG1RgohkRLSNCbZ20VvbZNxvY4JmWEYhmEYhmEYbyfLdgfh7j7x7gEIiQbQGltICeMR/XGAH1o4WqGCcd2EzDAMwzAMwzCMt5Orte1qQdQbMogGZEoZkBI56nbRvSGuAjUc4Xk+lud3TMgMwzAMwzAMw3g7OSi7nHWxowA3SRk1u2ihkbZt49QbJMLFrU+SaolIlG1CZhiGYRiGYRjG2ykg8QMZIcoZUtumNjVPgkYKyyIVmvJ0gzgcsXewi3mr3zAMwzAMwzCMt1uqlb29s8UoCCjNTqOiEDTINI5JhSCwbIpTdTJL8wQ69k3IDMMwDMMwDMN4Wzl24M/NkJ+fI9QJ0ncQUiClZaMsiQqHRLkMhYkGkTRfsWwYhmEYhmEYxttLKU1pYZ406+MozSAIUEojpUphPEIjSOOIXL3yHr9SXjUhMwzDMAzDMAzj7eQUixtOLvvRaDggTRMcaZFEETINAuLNTXoaYqXeM1Kp/64/bK6ZkBmGYRiGYRiG8XZ617/b35QZryts+z1DLeiv3EAlCVLYNpbtEN9aQwrBe76hnjDhMgzDMAzDMAzjHbGQ+ZP0nE4SP721QU76SMvCFo5DLpNnrjFJmiTmhX7DMAzDMAzDMN5RPAT5chU5iBg7DjIMxugoQiQp0SjMffsXs/eaMBmGYRiGYRiG8U7w1EfcB8PBqJbGKbo3RAAyn8kzlD6dsSY7HP9pfnvl4ecetR4y4TIMwzAMwzAM4+10+UP2Q+OD6x9MM/EfdPtjsPLYwkZmbI+0WEa4PnYwpJoM/2n/+ouf/N6nzB0ZwzAMwzAMwzDeHld+pbJsXXv543U3+c3xsIlTykNtEuk4yKDfp5QkqH6LcdAFKahJ/3PdF68+YkJnGIZhGIZhGMbboXP+hU+HHn8vN0yR3QH9cRM17oFSSEdLGPTwnYRR2Ke5scZ4ZYP4xWsfvPJfzJVN+AzDMAzDMAzD+Em6/LfnyurmrYd22rv0b6yT7uxTLPlYYY80GCPHnS7JynXyvqQ4UyNOEjKjmHnbe3C8svIBE0LDMAzDMAzDMH6SwpXV97lSPZQNYpIgZnbpCIU0prd1hcFggLRtBzXosb/yKkkQMHX6TlzbR/a69G9eM4+UGYZhGIZhGIbxE7X3ykufaIs+Pi6NO85Af8zoxirt4QFxHCOl55HaglanycHWHlF3iD0zgzfbYHf11Y+88IncWRNGwzAMwzAMwzB+Eq79Qube6Ob1D2aOTlBpTDHudOnuHNDa3EM5gmw2i5S+j3P6JLWJCSzt0j/oEzoS97ZlKvVi/cZTT/0jE0rDMAzDMAzDMH4Sbj391D+qV4v12uIUkS1o9UekwqPamGLhtlNkczlkQopdLlBfXKJSmmQcQrPfY9zbZ+bUMcqj4eSLv1w8bcJpGIZhGIZhGMZb6eKvNGYdHefqxxdQvR43Rgcoy8fLlsgeWcLKFUGAVHFEa3WFgWXh58pUKhNYjTqDrECoiNly9eHu5WsfMiE1DMMwDMMwDOOtNL58/UPTjfIjsZ1ihSnxYp1CtkyuMUvo+exfv0kUhkhLCMTNDur7Vxg1b8GsJDvtYnku4tjtiDNn3zMqZg+e+mRj1oTVMAzDMAzDMIy3wot/Y6be95U9fuCud+uzZwmzRY77BexJ2A5XaT/zLP6tIQKJbVk2lWNHaXf3aa3epDjoQtaHyUlaO7uUTt916UN/Fj/NM/smsoZhGIZhGIZhvCXu/KOtA6j87o176+XRc8+TGUK6sc4o7NPqd6n6JXKT04wvvIgdhyGIlNhWpGHEcH0Xx/FI90aoco3EKn0A3K+asBqGYRiGYRiG8VYbP3vlF0Y3tumNW1ijAVYYkI9C7MUyiZ+A1kjhOKj1XTjoUZiqkbvjOOXbT1FIPfzrB2x8+/m/94OPVpZNOA3DMAzDMAzDeCs995+VTu5evP6Jg60twoKHc3qZzPIRGpUJxG6bzq0NQGPbto2YqTPaHZHrDBhvtRmmAlWqUL3nxD9ZWDjy5Jk/ba+YkBqGYRiGYRiG8Va658+6V155952/KzerLzQ3rv9m+8pzeEKQKRYYFVwasxMMxGVstNYCKfLSI01TnOkSpZlpVLFMRzjNdz+ZnjPhNAzDMAzDMAzjJ+H2vxKPvfyu4uR0/hg6nGSnvcvBeIgtJM7YwbYd7CQMCF5dI1uvIe+YYVgUDPN5RGhRX7r9Kysn/fzqlRsfcObnv/e+P9o+MGE1DMMwDMMwDOPNdvVDE2c7g70p63jjZc+beHz88ktYmQnE2CfTH5Jf7aFfWmc8HGJH47GwJyewT51AFW1GvT2GgxaZQo3OyrWNXmuEU2l8tJj1Bia0hmEYhmEYhmG8JarZlf5B797o0v43dMYlFySMDvbxyjkq0/M4boKK1oiffw6Zr1Sxb1smzFqk7THu9ph638IZxoCmHfb+ufat8K4vrAYmsoZhGIZhGIZhvBVO/NvVQdTIHoiD7j8rNAO8IeQCG73aYrTeZuxZiPtPUqhUkFEcQaTZWduhFQzQkxUUkFce1ou3yG735x56QjxuwmoYhmEYhmEYxlvp579hf83dah0brG+gih6RJ/AyBexuzN6VNUbDANBIJ9GEV19Bjw7oN28CXZxJHzXcpbm9yfz97/lnJpyGYRiGYRiGYfwkNH7qPb/V3z24qHb3yFYddD1ha3ST/LhL7plXGPX6SK01o3BMppBlYeko+WKZ7totNtbWyM5NfSmzMH3ehNIwDMMwDMMwjJ+EO74dPW03Ji4drK7RvbVJ3s8yt7iElfUJZIIQEoltYVcrVGamcd08w9VtkkFIK4xQR+fPL3/hZmJCaRiGYRiGYRjGT0ru+JFzfQ3pSHFwZYNspkT12FFGZQ9hSaTSisLcDKnn0dvaY9CL8bMVRKlyLrs4/7QJoWEYhmEYhmEYP0nWscVzUTZ7znVLyJFmeHOTYRpSPHUE27GR0nVRGY/hcAB+jqmjp8g2ZrEmpi/e89XxBRNCwzAMwzAMwzB+ku78w+6V7NTshUx1iuriSaxMkUEaM865WLaNJJshKRTwMllypSpaW3QT/uHMmTu+ZMJnGIZhGIZhGMbbYeL0HV8aSefXcTP4uTx2JofMZ7FzeWQcjBlvbiCFS+zYBMX8R/Xk1MX7vhaaF/oNwzAMwzAMw3hb3PHY+IIqV9Zi13s0yfigNaOdPZLhACkch7xXII4VSdan6cL95/QTJmyGYRiGYRiGYbyd7vmefKxjgy6XftHGomz5SCmR0rGxHI98Jkfsux+unTn5dRMuwzAMwzAMwzDeCR74nvvVCGVlHRcXFxDIKBwRtlrsd9o4xfzO6f/HDfOVyoZhGIZhGIZhvGNY5cLmoD9ivNcmGAyQsVD0shbh/BTJYDB17WHvARMmwzAMwzAMwzDeCS59xHooHgflQaXBQSaLtG2ktB38ao2cn8UT9jesJPF/8LH8WRMuwzAMwzAMwzDeThc/WV+MU217tvuXOc+nWG289k6MFDi5HK4Cy8sx6va+LePENyEzDMMwDMMwDOPtNGp3llRv8JdKSnzbIpvPHf6dmDAMkalGjgLcVNFf30REZhFjGIZhGIZhGMbby/YyB62NDRiHKKXQWoGUSADCCN/PEHV7JPtNLKR5ud8wDMMwDMMwjLdVGsf55s4OdprgKYFOEpASW1gWJAmdg32yvSGubSOlMIsYwzAMwzAMwzDeVkKLJOs4xPv7xLkCjuOi0hQ7Go+JO22EnwOlcYsFpGsHEJqoGYZhGIZhGIbxtnElVIpFSFOSfptE2KA10stk8CYaBI5FaglSUuI0Ne/EGIZhGIZhGIbxtorjxE+0Rnk2IZJcvY60LKRlO6AU08tH/j/s/VmQpWl93/t+3+cd17xyzsrMqqx56OqJ7kYFtETLQhYcIYPVQo0NCCywvR04LMeWI+wIO44ddoR9wmeHHbF9jrRtDVjCAgkkYYOFLJBaViNa0DTVdHVXdddclZXzsDLXvN75PRf4XJ0dZ+/YQWb2xe9zmRG5Lr53/3ie5//iBB693R2KLKoqmYiIiIiIHKaM3Gm3W2DbTCweBWOR5zkmiyKMZ5OFI/yji6SNJuD1lUxERERERA5TXBSOXW0STM+D7ZNjA+BYWER5imsZ4sxQffxJopHv6E2MiIiIiIgcpsB2+8GJ88RWQJRmkBps28a4joNbFERpQhoOwRQ/6tXLK0omIiIiIiKHqVwbv+OUKj9apAl5kVLxII0iTDQaES2v0MkKPGM/5VZK7Se+srGkZCIiIiIicpge+eL6juX67cKxH85IaD+4Q5YkGMe2yY1N784dEgrnqT/sX1EuERERERF5K3ji64MrWThqDu7cgCzCMhZOludUx8Y4Um0SGEUSEREREZG3lsA26fz8LM6wx65tY7BtyHOyKKS/vXnxzQ8Gl5RJRERERETeCq5/oHkubu2cS0chFHx/xbLjeoT9Hk4eU/PsXw83Nx579SedZ5RLREREREQO0+X3+ZfCreVLJcf6TdKcZJRgGRvjVSoE4+P4xqK/s02QF/9heH/50vWPzc4qm4iIiIiIHIbrHxs7lW7dfU8QDX4zi0fkeY7TmMBvNDBJHGNlEUkyxEli/DTDt6x/vfLaax9ROhEREREROQyrV1/5pJMO/qXnFcSdNqQWVgLpaIixRzH0UyIKHtg9Hqxdp76xRnLllU/e/OjkovKJiIiIiMhBuvuJycX8+t33lDbadG/eY9TvUfgpcbxDGkYYqzsgv7uJZ0rUFybp5z1GexvMutbF6P7t9yqhiIiIiIgcpMGtm+8fx1xKWj3ypGB6/ghuEdLfuE+SJJg8jYkerOG/ucX8WsqFi4+zNxmwVUS0HyxrU5mIiIiIiByotfUHTy9lXXYmytQfOkvc6ZMv7VHcbhGHESaKE+xqhdH6NtmdbbJOn4nFBSab43RuLz3z6s9N6EqZiIiIiIgciKsfql/cW1m+1JycYHpmiiJJ6ba6tFY2cdwyWBamNjaG88hpevM14orLaKuDPYSxIyeY8Ounlv/ipV9QShEREREROQgrV175ZNN2Tx0/epIKDuFWC8uANT1O7dwxavUaxjI2ZqzK+NvO0B5zGYUhxVqLIhoxee4kxebe8Vd/dmpeOUVEREREZD+99OGJxUGns7B48hRZr0t3Y4P+cIBV9pl/7ALO7DiWBSYZDIjXN0mtjOrJOerT4wTT4wz9jFHd4ejkzLPJ1fsfVFIREREREdlP3aXVS43x5nNF2WFkxThHxqgdnWbs2DzFaES01SJNM4xxXfp37rH+0mXy9W38Rg0WJuhPVXDPnaB6+uw7yrntvPyhGX38UkRERERE9sXVj54PvNByxk+fep9zepGk5lFamMKp+US7O6y99jrhrfsYCxzjeYyfPIa/u0n48jXaU03yxRnySo1odRf/6MXLF/8kfIkbmyorIiIiIiL74uHPXQ/B//ytHzpWDV97kUpqMbi/Sntjl6TdpebYNOZn2bFv4uRJRl5xSdsFzVZMmrS53m4xXp8lryRkbfs58D6vrCIiIiIist8Gb974yWSjR37/AdH2NvVGg1Yeks81oG6TZzlOHqfkr17FxCPCR07C2Din/Abp0i69N25wq7P98W986L3X3v17/StKKiIiIiIi++XP/0rt4vLXv/PRfH2Jo+dP0njoIux0mdnbImy3WOtsYOUjHOO4OEfmSHdX2IsHdO73aQ4MAS4zj56mdubo55/QACMiIiIiIvvsR/5r79qNdz32m0Vr5gP3b99gcOcek16Jhm9IrZzmkTmG7jqO67kkVZ92VsIaZSxUpqicOIJdcYnLFvWLZ/6QF9ZVVERERERE/k+5+0/f87/795P/4vn/w/91Hz/1p4OXWpx96CJRGLG9t8n99jZVz2PKrhG7LiYbDVltbRI1G0xNH2PqobdRalQYNm28R08Bmf/Hz4TPfedDs9pOJiIiIiIi++LyB2YXv/GXi/fGbhpULp4CB/xqibG3PUr9oTMMg4D+8jZpFOOkcUyz1qA4fpJqWKHY2WavmtK1DMm9e2Rb0YpH5e/+0AsbG0orIiIiIiL74cmvbCz9tyfa733wnY0/mnAtvCKEOCfpWSwcOUreaBJeuUUcxxi/XqN57AxjiUfWH7E92KNdxJQKg9caYrYGv+wH9RVlFRERERGR/dQojy25a91/5w8S8Awjk9Nf2SS/t4kdGeqPPERQLmOwLdhqs3tziWjYxx6vYRcwhg8rLVoP1p98x7fdryipiIiIiIjsp3d9k68NlrcvDlbW8QOf3CoY8yswSOjcuk+vs0tR5Jg4DMmWlul399jY2yIIfKaDKsnmHlsrq8y/+53/VjlFREREROQgHHvX0//L1vomUafHeLWB7zh0NzfpdPbYunePKIowxjIM0ghvss7ciQXKrk++vsfeG7fIqsEV/5ET31BKERERERE5CKXziy/a9eqLm1duMrj9AGeiSeXYNOWZMdwsI89zTBpGmMk64yfnCabGGC4tU9xdZ8yvMnbx9Nce+g960C8iIiIiIgfjzK896I+dPv7ChF/FaXUZrNzHOzHDxJmjjJUr2MZgwrpHdeEEWVane3+bdmePZLbCkhtRWzz7h8ooIiIiIiIHqXzm4S9uOvlLZrJOuNume22JKCzhHb+IbcoYJ/CgViUORwz7A+YfehhvcpLG4ol/60/MXlFCERERERE5SI/97uBKaebIFRrjNE+cxZRrxKOQ3PexbAfjlMqknoNtcsaPLxJFKbFX+0eVY+f+8OyvrbaVUEREREREDlrjzCOfD4PGP8htj8bEBJaVYZUc/HodM9zdxeztYFxDHMVQH/+ZbGz2yhNfy59XOhEREREROQyPfy17IR+bvZJWGh/sRDFeJSBvbRJ1uxivXMFUS1ieA4H3P41cv/ND3+BryiYiIiIiIofp7d/In+/b3sCtVD6Rk+NXyxR5jnGDAIzBLZcpKtWNJ/6s0AmMiIiIiIi8JVz6RvF8FgRtv1LG9j3cwMdEvR7DrR1a3QH+xOQNZRIRERERkbcSZ3z8Trc/YGdtgzgMMRYFUVZgpuYIu/3Z154dP6VMIiIiIiLyVvDyT9cvhoPBZNEYp7Ac8izDuL6PPTkLhY3XGPuzNB41X3luTIOMiIiIiIgcqu9+bGLRJKNmo1H7M9fx8OrjOLaDyfIMu1Ij8MvkxpAO+t+NB53jSiYiIiIiIocp6bePJ4PeN9M0wXc8So1JbN/DZBZgFeRFhNfeIb+9RDbsz/yf+dHX/vHxQGlFRERERGQ/ZJGdphvb2IMBBRDZFrFVYCgKHANFkZCEQ7LNLUrVcuv/zI/alpUqrYiIiIiI7AfPtsPdlQdEwwFxmmClCZZl4aR5jpvGRL02w80NyBOcogCs/8Mfvfgv72mIERERERGRfeG4Tui7DvHONmmWU/Z8BkWBY+U5o40N8mYd37Ux1RKmyHxwVE1ERERERA5NEoZNx3FoVCpsd7v0HZeiKDBBrY4/NckgSyl8nziJyItcE4yIiIiIiBwq4zghlsUoTYiLHK/ZxDI2JgtDjOczc/wkFDZZlFMYJ1IyERERERE5TKMwbPZHI7IgYGLxOJ5tY+UZJityBrlFhk914TgjyyHLFUxERERERA6XY7thbjlUJqYxjiGxDVgWpgAcJ2AY5hROwMz5h0kyowf7IiIiIiJyqLwcFi9eJPU8hnlGbBkAjBf4pGlGFNTotgdU6xPvs93SjpKJiIiIiMhhcsvlHcsPPjjq9RjaHoXtYFkWJmv3sB+sEKQZVq12PLLz8Kk/GF1WMhEREREROUyP/Zf2jcixI9NsPuymGYP7S6RhhPFHBaVOH/fa97DD3dMXvpG+oFwiIiIiIvJW8I4/tr6W5GEzv3uTma0dTJLjmMDHmp7ELnnYrhNCrFIiIiIiIvKWEeCEVqmEu1DFq5QxcRyBneLUysTbrVNXfoz3KpOIiIiIiLwVXHm/f4nNnXNBJSB1C5I4wtiBT5iMaA33KAXBb7qD4cQb7w8uKZeIiIiIiBym1/9q7aK1vnXRo/hcmibsjboAGOP75BNNMDlxu4WbRJ/r3r713jf/1tyksomIiIiIyGG4+rfmm71bN99fKfJft8OIaDTCn5nGr9Uw0WBAYNnYxiLq7mG7FiUr/edL3/nW31M6ERERERE5DEvf/e7frlvZv7ZMTm9zC98ylI1LniQYuwD6Q0ppzrC7x+bta6RRj97dO+998+fGTimfiIiIiIgcpNc/dWSyd/P6B6Jem/b928SDNoHtYHa7FEmCsaOY+PZ9yrbLzMIRCiKG7W1qSXKpf39Fb2NERERERORAxbeX3zWeZE+ne9tkREwuzuFkBcPVTUbDEYa8INxs0X2wSkHGxLkTVGol3DSiuPPgaSUUEREREZGDNLh6/YPNDCoVn4nTx0jzhP7mFp07K8RxgonSBIwh7AzYuPsAOyoolRvUG+Ns373/zLWfnppXRhEREREROQh3/+rEYry2csmtV3EqTch9du+u09tp4QY2xgLj1qvUHzqHU/Lxq2MMtjq4JqB54hxes35x/fVXP6GUIiIiIiJyELaufO+TTtW5WDo+h1UZY7DSp+5NUvYDxi4uUm00MK7rY8aaNM4cJ89D2lGfdq9NFo2YPbVIFPbmv/1X6xeVU0RERERE9tNLHxw/tT7aPd14/CyhSeltbeK0ezhJRunccezFKSzLwiSDAUlrk8zKmDx3mtp4k/JEk144IHds7MD/9M7WpoYYERERERHZVxvdrYtHrdpHGp2CYm9APj/G7vEazttO4hnD8NYKYRjiZFlKd3mFvV6L2WNzlJtNvHIFzxnBsVNPzce5s7zVOvUXHz9afddnl/tKKyIiIiIi+8HqjprNi6d/nKbfd5dufrvRmCDNQ/KlVVbDLTr9FlOWwXHLZWpT06Rhm/adu6TA7ImTuHjES8uXHrk88ctQeYnvLKuqiIiIiIjsmw9crn0WCq5fuv8PTZEzfLBGdmcDh4LCH7KwOEfu38dYFnilEn6Wk7e7hDstbn/zW2w8WCHe3L54/UeL9yqniIiIiIgchDtPhc+Zzfbi9q37vPbSt+gM2vQHHao5NO2ANElwMDbdN68TmZSZmWmyqXHioEJ/o8W9pfufbq9uPPYXH/zg/Xd9uXNDSUVEREREZL9886/UL974vT/7RX+wc8k7s8DxH3s3lX6EWVkn7O6w/b03yPMcJ08yapOTZL0dRr0Ow94ue4MRtblFZk8c5ejiyX/3dg0wIiIiIiKyz374v3avvfzjP/Tvuhu3PrfX3oVvfoexOMANfLpli+DoBMXLyzi242I1qvhJhzAcYlfLLC4sUJ2dZ2hKBI9c+Ap/fl9FRURERERk302cOveV2nAPZ2qCTqPJYK1NvtOmEZQo2QEt28Yhi4vi3q7VbjYYPnqc5vgEVatMGoZ4F85gFQUvv8e8Jxsfu/2O320tKauIiIiIiPygXf6Z2dlob+Ox2ONF7+GLFDdu05w9TjQV091YY21tgyPLQ4osx4l7Pcuq1Jm6cJbBsTJZr0N7t4fjexRLD4i68ciKnE9NzM5eUVoREREREdkPY4HXX93ceKw96v6R79uY3CLvDzElw+TZM9QmZkjfWCKOI4xXqcJDixg/w7nxgP6bd4hGHdIkxBul5Du9fxU6Jj3zubUdpRURERERkf1w8nMP+sVY436xuku5NcJKIjpRh837S0T3VylhqJ1dxPN9jG07RCZkZfkWpVaPWeNjlRyq9QadpTW2lzce+5FXa59VVhERERER2U+zD5//SrS8+bVkdZuy6+AGDs1ygDUYsXvnDtgWXuBj8igudu7dYqe1QbS1iRu41KtVws1dks0Ox9556f+lnCIiIiIist/O/vulcOrRRz4fbmwR7nRpBmVKpYD1nQ36nS7br71Bv9vDZBZW1O4yMz5J+fgCZnKcdKvD3veu47vBl7yHTj2vnCIiIiIichBKbzv3paRW+VL71TcwG20qk2NMnD2OM9bAG2XkWYbJspTp8Wkm5o7CkSk6W1uEqy2alTHM9MTVC796N1VKERERERE5CCd/Y7lvLUxdLVeqRGs7bK6s4o43mDp9inK9iW3bGGPblCdmCEoNdrZbdHZ7+G6VNvnz5cfO/aEyioiIiIjIQSo/fObrvYrzghX4jPZ6tDf3yFwX9/hRbMfBWLaNqY/R7vRJC4fZ+bNUpqbpzDRWH/qD8CUlFBERERGRg/Tol+MXOzONJWYnObZwBrMzYjCIiKfruEGACapV8kqVIqgwXpvAS32iOP9XUz/8uDaSiYiIiIjIoWi+4+LvD9z835lBxszsSTLL0JmuY5fLOO14gLWxTqU5RlxyadW9j6blic1nfjfXg34RERERETkU7/6C85Xn//KFwWo4fLGSRV8oJznO1Ru0B11MmqdYtTJxmpE7DlnJ7zzz9UIDjIiIiIiIHKr3/HH+fBaUWrEFxrIIqg2yPMd45TLYNn69Su44H546dvwF5RIRERERkbeCuePHX7C90s9Y5RpYFhZg8iyjv7NNNxpRmZi8ceHXl/tKJSIiIiIibwVnf/VeOjYze2Wn06PT62IsC2NbFrGVE1UrhGFUffWDjXNKJSIiIiIibwWv/ez0fL87mGRignaakhcFBmMozc7gV0tYvvfNNE2Dl5+dmlcuERERERE5TN/768eqSRhXC8/7dqlUonnsKAVgirygCDyMbZGTYzq9V60wbCqZiIiIiIgcprTXXkxau9dNDiXLIioKHMfBkOcU5Fh5SjlNGXQ6ZL3egpKJiIiIiMhhytM0yHo9nDghiWM8Y2NZBlMAXp4RuDaEIYOlJUqep8f9IiIiIiJyuGyT7t2/D+EI43h4QEGBU1gWrmPR29sl2xxQRBHGMqmKiYiIiIjIYTJF5nhpChtbRBNlStUA2/NwsihisLpB4VewioBqqQYZjpKJiIiIiMihDjGZSSt+gGcsTK9Dp9+i6A8xjutijzUZuTaRDZkBy7ZDJRMRERERkcNUpGkwKnISxxDaEDSbGNv5H9fJqnVmxsYI728S5gmJQdfJRERERETkUKW+G+YW2L7D+MwcxipIHAdjAWGckMcp9sI8fdemn2rFsoiIiIiIHK44z5zQMdhzcziOwyiOyIscU2QZtjHEBRS2w9TZU7iV8o6SiYiIiIjIYXKDoD155gxpKSBMMhzLhizHGGPhFobIciCMqI013xHbCiYiIiIiIofLuN7AGms8lQ+GRI6Dl0ORpZg0y8i2Wzi4UK6dHmVp8CP/pXdNyURERERE5DBd+t3NjRiLJAjOe7ZHvLoFBRjX89hb22Rw8x6M4urT/y17QblEREREROSt4Jk/HF5OR3G1d+MOnZ09ABzLc5leOM4waGBnub4PIyIiIiIibym+X9qxShWCWY+hbWOyUQQWFOREu3unvvs+/5IyiYiIiIjIW8F3f6r85HDlwdOO7WJnUOQ5xumG9NM+Iy+igfWFsbXti288W3lMuURERERE5FAHmI87zwTrbzw76aSfM2nBdlCisCxMeXwMf3wcOxxRRAMsl19v3bn7nit/e76pbCIiIiIichiu/M3FYPfarZ/wPe8fkwwwWchYtYpbLmOSKMSLE9w0ZDBqQcnCHwz+Tf/l1z6idCIiIiIichh633vtI5Oj4h/bZPT2tsmyIaU0IksSTJKkMIqwyNjtbLLz4BaNMCK7ee89tz92ZFL5RERERETkIN385PEguXnr/fV2n97SMr1+G4sY2i2SUYjJo4j0/gMqgc/8yWOE/T16q8t4vf6z4dLyu5RQREREREQO0nDp9k+Yvd1n0/VNrGHEzOJRytWAaH2VIssw5DmjtQ16G9tEwyEz585Qmx7HSkO69+6+RwlFREREROQg9VbvPROkIW6zSfXUWfLUYnd7k53lJZIkwuA42EGJne02W/dXcUYxTsnDbQbsra9cuv6xmVllFBERERGRg/DmJ8ZPte5e/2C1GWA1KpDkrC+t0dpuk5U9kiTFBOUS/qnTVKrjlK0KST8kKjs0zxzFtri0+90rH1dKERERERE5CFvfvfxJP4tPBWcXSEsWYXeAm/k4bpm5hy8yNjmJKSwLe2aGiYVTlEyJ7m6X7V6bnpVz4txpspWti7c/eiJQThERERER2U9v/NwJp1jdeOzE6ZOEAawN2rT7Mb5dZeHEeUyzgXFdTDwckt5/gKlVqJw7DrNNirEqvSwmsgqqtcrHd69dfVZJRURERERkP7Wu3/xApdJ4v+PX6HR7pGNlWKjTuHAUJ3CJt3YZ9ds4paCUh8srppOPqJ+exZ4fp2wbsvaQ4MgprGMFSw/WZl/8qenHnv6DwRWlFRERERGRH7QrPz01v/fm1eP1Rx8nKRm8zdtkQYHtGgbdNUZ3tvGW21DEOFmamWBuknjYx/32DarlCtbcNPFUg+29NZoPP3Jk7MRCu8gKBwaqKyIiIiIiP3CP/eft1Wt/59IvU7b+3dob30umApfadpf09ir0BnjG4C/M0LlWwSmKAqdZo0h67DKg2G6R379B4/hpxsZnsdLldz35Te9LyioiIiIiIvvp4r+/H177kfjZuTfX8ba2Ga7dI686DMZK9GseJ+bLZGmCU6QZ3Wtv0Lcj6guzWLOnsRID6wPaV28Rru397e+87z13fuiPdJVMRERERET2z7f+b6UnV7/+nU+WtzaZWJzGPvsEhRfh9wf0VzbY++410ijGwbGpjE9CdxurM6K/e4thVhDUxuDiAvWF4196SgOMiIiIiIjss3f+t9Hly+946Iv5g/L7u61NkhsrpMS45Qply6XeqNNxHBzLcbAb41TCiO7mLrVGmdLsOJxdZCcoUb348Bd5eUVFRURERERk33lPnfnDrj+kudAgv3kPs9sj7SYkZQ8zPYPjeTi2BdnaBnHgkp9awLtw/PsbAPKCxaOncWMn/Mb7/EtBY+LOD31hbUdZRURERETkB+31D883R9sbj1Uy/yXOnmHjzus0fvgsXickurVOsDMivblDHEU4WRyTV8pEp+YYu3CUe3trOKOI8aiMe32HdPXWyA38v/9DN6OXlFZERERERPZDUrEYXNt4bGxl888q402ODlx6TpfuRJXaDz3KaKODubVKHIYY47iYcyfxp6boX1/Hf3mV6ZUMvxWSpxErpeSfDKZLuk8mIiIiIiL75onPrLTz8fLOPXvwL0bxLhkF9m6G/eoq/q0WE9SZOPswQbWCyW0Le1QwunaPuLtHabZCUorwZmt015bZvH3/3T+uFcsiIiIiIrLP3vONyud7D1Yvjbrr+GM5ebmg7pYo9oZ0b92AvI/rVTBhr0t+Z5lkbZtBd4eoFONNOUTtVfZu3+L8+YsaYERERERE5EAcO3riT3euXyXsblBtuLjVgM7mOsPuDtu3rhINYoxrmWLY3cGZqHNkdoG50iT5dsjy9buYRv3F8qMPaYgREREREZED0fyhxz/rTs1cvnvlJul2H6fZZPLUIn6jQdgPCcMRJgojK2uUqBybw2tOMVxuE690sPAwJ469eO63tZFMREREREQOxpnPbm7YswsvlbIyw7U9+ru7WBN1GotHqXkljGVh/FqN8snjUPOJ1rdhvU3g1sj9Kv6xoy8qo4iIiIiIHKSxE+e+4pgqvltnc2OdrbBHVg5ozByjXC5jjG1j1auEZESjCHf+BMGRRZzxqV+ZOHX660ooIiIiIiIHqXLs1PPl6WP/rjw7z+y5s7RaLQbhCKs5DpbB+JUqpl4jTBOCI0dwp2eJo+SfTJ2/+KUz/34pVEIRERERETlIp//93bR+4eKXItf5+8axObZ4nCyDYnwcp1bFDPf2SDc2wDJYvkNipWTzRy4/+t/5mvKJiIiIiMhhOPdC8kI0Vt3IHUPF9zE4DDc2SPo9TE6BZ7sExiUOXNpV58P5iYWXlE1ERERERA5T+bHzXzEWfz/rDggcH6deo8gyjBf4kObUy1Uim09UTx978ZHPrraVTEREREREDtO5X1oKS0cXXnJw/r4XlBhSgGVhhkVCO07ZasfUp46+ePHXtlaVS0RERERE3gpO/k74UnFy8YVRUuBtD0mKApM50BsMSctV4lbn+NW/1jinVCIiIiIi8lZw5VPNc7v9/mzHcem1e9i2wWRFQf3YAhONGs5Y80/6g/7sN35+YlG5RERERETkMH3n41Pzo3Z3wavX/miyXmXi7AkwBuP7AU45AMfGGg6we/0/iweDWSUTEREREZFD1R3MMgj/xBoMcMo+kWe+/yamKAp8YwEprmXR7/TI2v0ZFRMRERERkcOU9oaTw70utucxykKcksGyLAyAnScYqyAZDRmsbVB1g46SiYiIiIjIofL8duf6TZzBENe2sJIhljHfH2JyC8Jum7zXJcCihElVTEREREREDnWGMSYdr9XJdncZ7Wzi+i4UBU4WxXTW19hzAxqZjZtkOEWhYiIiIiIicqgKq3CKNCUZjWgNukR7Xewix9iBj9us4QUetuNAFGNlWaBkIiIiIiJymHKrIIxCvHIZ1zgE5TJYFg55QXmsSblUxb67RWLZGMfrQ6hqIiIiIiJyiEMMuJbBK2D+2DHyJKNrWRjXduhGEd2ST7awyJ5VIR/ajpKJiIiIiMihDjF51Ox7HuH0AoldZ1CUvr+dzLJtqrZP1o8oyiUaZ06T2kbHMCIiIiIicqg84/anT56hcH1GWYZvLIo8xxRFDv2Qih2QDXp4E5V3pFW3rWQiIiIiInKYLLe64VUaP5qPhhiT4Wcj8izDZEkKO238MCMJ/IdbRM23/5fdO0omIiIiIiKH6anf37sTFVZKpXzeLRI6myukaYopKMh32uzdWyaNo+p7/5ivKZeIiIiIiLwVvOtPkhetOK6O1pcZ9NoURYFjex7+6RMUmYNVFHrQLyIiIiIibylWkTulIGBs/gh7vo/J0wzSEGNSRq3tc698wL+kTCIiIiIi8lbw8k94T8cb609aWYHBfP86WZakJP09CAoqhl/Pl9efvP6z0/PKJSIiIiIih+nNvzaxWOysPen77i85tsOo3SPPc0xlbAxmxhmkfYrhkHGn/Evdqzc+qGQiIiIiInJYbvydo9XWrWsfrtrF/2pnCcNOl9LUEcrNJiYZjnAMxKRkcQzhiCCJqi++M3pW6URERERE5DBsvf7KJ924/689U5D3OxSWBcYiGQwxXi/E2k1wEsNyuMbO6DalcO1fc/mFf3rr5ycXlU9ERERERA7SzU/OzkZXrz03trtLtPGAUWuNpBTTGW1QhBEmHY0I760yXh7nyPgsvZ09Bjst8jB+rHdv6WklFBERERGRg9S9v/SucDh4urW9Sbi9w9j0ApNujXh1jzwvMBZ5kdxfY3Rrjapd5sjpM9iNOrg20YO1J5VQREREREQO0uDB2pNBYajOzlI5dQKMIVnvEN5cJ4kTzGgUWp7tMdzYY+vBKoWx8MbrBKUy/XvLT7/5sdlZZRQRERERkYNw+xPHqvH9lUt1v4QTBBB4bKys0Fpao5G7FFmGqU1OFP7ZM5jAox9F9KKIuOQxtrBAkHOp/+qbH1BKERERERE5CK1Xrz7njuL3TB49jvF92r0ufavArVaoH12kMTmBsR3XYnqM8vnjVOo12p02a1sbFI7DkcXjZA/WL1378NykcoqIiIiIyH66/omFZriy9uTs2fOQW7RaLTrRCLdepnJyHnNkkqIoMGG3R9ZrYeo+M9PTzI9NUy3XaId9KHn4lvXJ7q27P6akIiIiIiKyn7Zu3vkxy7E+7fk2cTTEq1SZqNQ4MjNFpV4iirsMez0cx3bYWL1PlveYn5yjktuUJqeJCjATC1RnMjp7e83nPzx77j1faN9QWhERERER+UG7/KGp+e2b12fnHj4HgYO9mjLm21gW5FHK7r2b9NZb+I6DcQMff6JJ2O0QX75Kdm8DBhGJ77A36nDmG4U1+dRjn9UAIyIiIiIi++XJ39tenX/i0c8/+jxWK+5T1EoUo5jBjbtsX36NcKfFkYU5glIJJ00yJpp1staAIo3p3LzJ5tIN3PMnac7Oc/39jWfO/8elF5RVRERERET20yO/sdp+5X28d7CxQXt7nfabd5mOC8brdaySh1f16GcxThKndG5cJYw7lKdP4J89wbEwJWm3GS1d5u6d5f/7d//K+9pP/dfBFWUVEREREZH98sp7K491//jr/7MZbVOdrNJ47BGqqYPZ2KTTWmMvbePmIcYyUK2O43sN0mFCurtHa22ZmBT7+BGa549/o7owfU1JRURERERkPz3xtcEV88SpP/XnZhnr5jjXlknXt+g4BZ16mUq1QZomOHbg4YxN4WQp7WFCHPepTU/SPL5AXrJxzpz//bP/271USUVEREREZL9NvuOh3x+V8n9tdzPSm/e51++SGYdZu0LTnyPzSxgrzxlst4iMS2V2nvknnmDubY9TuC7W7BGsZmP1z37ae1o5RURERERkv9z8yNng5Q+Wn3TK1U33xAL9iqF+6VEW3vEEweQErlWGzQ5FnmPyLMM4NmOnTjH98CMYx2bY7REXBdYoZuOFb+2FazvnvvXRY1WlFRERERGR/ZAWmdNb2bzY+u+Xe95mlyLNycKIilPi4Yceo3bsKMOyIY1jTF4UlM6fxWvWGHZabCwvM+p0cUcx1toWQWz9s2pqh+/83IO+0oqIiIiIyH546Lfv9EtBpVVa7eHd2abcSwi3O3TfuE+0soU3P4F96Sxe4GM8z4MsYWPpDnGvw3itQqWAwK8w2mnz4NbdH/vhy5XPK6uIiIiIiOynqYXTL3ZWNp6PeiPcRoWiZFMdq5G0O2y9+SZEI4xtY4q8YHDrBnsby3Q21vBz8N2ApNdjc2ONqYcf+pJyioiIiIjIfjv9hbtt520PffHV7bt0kgFuYJM6Gd3+LvHyGsmVWwz6A0ye5WSdNvO1KhMTY5RmpknCIcuvXyEqshvTD5/9qnKKiIiIiMhB8J848bxX8i/3X3yN0tqIZr1J7cgY7myVuLuLVRSYPEtJ63XMwgLVsUlY3SBcWaNcqVE7ffar5z+7e0cpRURERETkIDz1mb07c4vnv1KxSyQPVij22tQmxhk7tkjUqJPnBlNYEJw+Qzozx2iUED1Yo4hSBoVF49SZryujiIiIiIgcpPrcqec7pdILo7JNZ32VYaePE9QpLZ7CC6qY3BgcJyBpD4gGA4rFRRoPXcBMT322snD0RSUUEREREZGDdPGP4xft8fE7zTPn8BaOksQp0WBIrdrEcV1MqV7DGZ+i7JYIGk2C2Vn6Bf+sfu7sV8/9yrLWKouIiIiIyIGrPnT+yx1j/lF5ZpagOY5t+5jmJJbrYNLRiGJtDTu3MeUaQwr6zcb9t//34ItKJyIiIiIih+HJ/+58pd9s3N9Js0+lXgCWR7a6AQWYLM+wMvC8EqntMHCcD4898ogGGBEREREROVTPfKP0xdDz+6nj49caWGlO1OthvHIZLIsizSk8/4OVEye+ceFX74dKJiIiIiIih23m3IUvZZb5mSiOsIMA23Fw0jgmiWNG4ZDKsXNXzv3WxoZSiYiIiIjI/1Un/8XzP7DfOver99KbHz5yebB0j/aojzEGk0URg51tQt9jtLd7+ns/f6yq7CIiIiIi8lZw5eMnnNFe53ji2Oy1tsjzDAcsxk4cJx9vkAS1P+n2e6cBbSUTEREREZFDN2jtnqqMj/1Z3c4onz7OyLYxVDyGtTphUIc4pbK8dfFbf7V2UblEREREROQwvf7+8XP1fue43e+RVupk1WkK22DySoBXLjGKU1zHIdze/XLe3dYQIyIiIiIih2o46E2G2xt/5AQ2oyLFzyywbUySpthZiu+CFQ1Zv/4GQbW6qWQiIiIiInKYCtuky3duY4UjjJVh0hiKHGNZBsvKsaIB0cYaE40KtpU7SiYiIiIiIofJ2NCs+IStDRj2cHwDgGOyjO7qA/ZsQy2z8IoUK4sroDlGREREREQOT9Ux6SBLGPW6bHd3KIoKYGEsx8GtVrGMwZQD8jzD9/yOkomIiIiIyGGKityxLYvA96hUqxjPIysKnMKC0tQ0U8bg7HUZBiXC0WgCPFUTEREREZFDU+SA42D8gNnZadx2QpjnGGMZhoMQUxunmD7CXpiQW+5AyURERERE5DAlOXTjHHvmCKPMUFgulmVhsjTB90v0+hGuW2Ls2Gny3FIxERERERE5VEWaO3MnTuPYAXFhkxU2FmAKC9I0wynV6HcHNMbGPpgZK1UyERERERE5TDXP73uV2s+M+kOKUg1jexSAqbVjorvXcU2HtOI8tTdeW730h8XzSiYiIiIiIofp4tf6V3YatdWiXH241usR3r9O2h9+/zsxYRiyt/wANwybT/3R6LJyiYiIiIjIW8GP/rfoJTtJg3BjnWw4wHNdTOF7TMwv4Bc2+EFbmURERERE5C2lKBzb86jNL+B4HiZJYoo4pV6qEK+sXHr92eY5VRIRERERkbeCKx8aOxW3Wqccx8UqcvIsw8TREGcUQZjgYf1S98HS01d/7sikcomIiIiIyGF64+eOTPYe3H/GSuPPFVFENBgQRSHGqzdIygHJqI+TxTQc69f3rr/5rJKJiIiIiMhh2n3j6nN1m193sxTP5GSujVevY0xW4NWqxPGQNI0oezZWu3Xqz9/Z/tvKJiIiIiIih+E7lzqfrCTDyWqR4mQR/d0dquNNbMtgiGIYDXB9h/76Cq2bbzIduP+wd/PG+29+ck7XykRERERE5EDd+JuLQevOm886o94/b68/YHf1PpZvw7BPFoaYLIoZbW/hVV1m52dw2y1237iK2+18IFpZvqSEIiIiIiJykKLlpWcY9t/fW39AvLPB7JFJxusVuhurRGGIsQtYv3eHXreNVRTUF48z1RijlCbs3LrxASUUEREREZGDtHXr5k8GWU6tUmHi9BmMsUk7HdaXlojjGBNnCfWwIH5zjXAnpLCr5NPzeEGZdGXp6Td/bvyUMoqIiIiIyEG4+7Gp+dqtu++ZHGRU6jMUTond9pDtm8tMtxMMYBzPY+yh8yQlj04SEo4GGBvGz5xiGEcXd2/ceL9SioiIiIjIQVi6/cbP3KyEF+13niGqOiSdIXFrgO34NM6foVQpY5wgIFuYoXRiHlPyGQ1GhL0emZVy9JFzbKwtPXPt5084yikiIiIiIvvp+t886fTuLT99ZvEsZpQRbe+yN+zBeJ3SmeOYM8fwfR+TZxluu8/U5Cy1YwsUkzU6dkpnb4eK4zLlBM9uv/76R5RURERERET204M3Xn9uwrjPTScGs9slM8B4hdqZ45TGx6DTJ0kSHMuC0as3SVp93NNzxLN1AsqUeiOc5jQLQcbd3nDyhedOzD/zxe1VpRURERERkR+0bz87Mzu49frs9IkFsppD3t7Gq5YpMgsGQ6KlbeK7yyRRjGNhYWbG6LZ3sF/eI6jXGFucgYpP1+pTf/rJscmtqJIW9U3YVl0REREREfmBe8eXNjcuP3f+y/ZC5VdWrn+rN1kxlPdG2PfbhLt75K6hfmIK51UXp8gL/Olxyq0Ee61HtrLN/Tdv4DxxgnCuRnfl3rvf/QX3KxpgRERERERkPz35xd07rz63957S1pD+/XX611fJEgu3USWbq2NP1ygAJ7MN1uu3CEZdooUZivMzVOKEuDOitLpH/ObwU9/86b9854f/c/easoqIiIiIyH755rPNc9f/7I8/PTbYpVSkNB9axBiXpNsjXF0ja/e+v2KZwoLAIvQzrCLBC2Py1h7V3pBmqcrU7NxlDTAiIiIiIrLffvhL7RszU7NXSkFAKSmwd7t4cQJpRFwuGJZz8iLH2EC8OM3gSBM6IfmbG1RGDt5D5ynedgb3Lz31K8opIiIiIiIH4ei73vVvG6dOUz1zgbxfkL+5it1LiSbrdM/NYGwHBysnXwsppVVoBPjnZ/AWptl1CkpHj+H6lZ1vP9c4F1Qrrcc/s7ajrCIiIiIi8oN25dMnnHRj99TpX12+8erHHmN1/Q7TPzWLc2ebdG2Tam9EtJrhFTlOnuXEToZ/ZpG6WwXbYkDMcJRQ3uvTf3UpSfPsfxpcWHwR0BAjIiIiIiI/cGFvNDm6u/TMlcft69aOQzUF24ox9YDyxGmSsEu4tUoSJziO7VA/fwqmmmS31mnv7dCeb1CqjzF88x6VoUep4jpP/o7exYiIiIiIyP649J82Nl54wgmTvQHlMKRUStka7eGHIxpOQGNulsZ4nVb5FUyWZmRJwtrNGxSkOPWAoMipZzBZGPoPlp9vPvLwF5VVRERERET20+T5c1/Zvr/0NTuKqRqHqusQuTlx2Wa4skKx08G2XUwaR6T3Vuk9WGezvY1T9Zn0qwSDjL1796lcOP6lU7+7qmtkIiIiIiKyry5+frU989C5Lw021on7Q6qlgGq1wsb6GtsPVinurTPq9TAWeZHdX2fCKRNMNSmPNXB3B3Refo1eFL3ovPPil5RTREREREQOQuXJhz+/HY2e37t5G6s/ohpUmGw0aTgBycYuWZZhojC03FKN5rHjNGem2d3rEm91cGyP6rnjL1z4jxsbSikiIiIiIgfh3GeX+8HZE9+wPI/B8gZZP2L8yDyVY4tYrk9R5BivVMY9dwIz0WTQHRI/2GA0HDGqeNinF15SRhEREREROUhjp088H1FQxDl7D1bp7O3hHpnCPX8ax3ExFhajsTI7JYtBq0u1PsnY+dOE48FX/KOzV5RQREREREQOkn907rLreV8dP7JAbXySThqyk45gukEpCDDOeAN3egybjOnpI9TmF+mkxb/yTp376sVfaS0poYiIiIiIHKRz/9tS2HjkbZ8ZOKV/Vj9ygonaDL5xsSabUC5hRiUbWtuU9gZYfsAQi+HU/OUf/ovmryifiIiIiIgchof/wv9Se3z21UHi/kyFJkE3IdvaIrfApIMBprCwq2PEmUWC+cT02TN/qGwiIiIiInKYfuQ7zlcyx04Tk5OXAgpjk3Z7GCiwnADL9kmM89HKyZPPn/nNpVDJRERERETksJUWF74TYX00Mwbb9nB8D1OkKfEwZhjnVI+f/NOzv7O1qlQiIiIiIvJWcO53NjZqp099PaVgEIZkaYqxXZd2p83Qdtjd3Tv1vb+xGCiViIiIiIi8VWxvb11sU7DX3iVPU0xh2zQXjlAfr2M3at9Met35lz91wlEqERERERE5bN95tnmu2qhtzE1PU56ZxvE8jG0MXrNKbBKsuM+gtX07arXOKZeIiIiIiBymP/9A41zUah+38/x66jmUxupYxsbEUYxFikVM2YFsOMAbhk0lExERERGRw2QlheMMR39kkxOHA3zPAAUmKFeIiwTby8nDPoOVZfI403UyERERERE5VIFlp5u375JnBYVjEY16UBSYIs/xco9SNyNba1EOY4LA1YplERERERE5VLnpTza8mGzpLqVeQtmfoMDCIcsYrO8yjDOcvKBkgZ0n2lAmIiIiIiKHy46raTYiDwfs3hkQlGuAhTHGULg2kQN5vUIvjjCuTmJERERERORwFYUb5hgs1yExOXnggAVOliR4kxNMl0oU27uMmg2yJNebGBEREREROVR2Bp4fkBjD9IVzFHHO6P/7JsYB0lKAvTDLenuPvFAwERERERE5XHGSO3ujEf7RBYaOwfFsLMB4lQp5UdAfjHCqFaYunCW2tJ1MREREREQOl+044dSJEzilCjYWRZJiGYNJRkPsDErlGju9PuVmnZGtYCIiIiIicshDjOe3nVqNKI6xCgsPhyxNMXG/z3BlEz93sbBJPOen6vMz15RMREREREQOk5kYv1OU/Q8WtqGEYe/uMkkYYtxSiaTTZ/P2Ek6c/ujY4uKLb/u1tR0lExERERGRw/TEry+FYyePfyNL0h/tLW9Ad4Rt2xg8l9rkEcaCKgR+++FfWmkrl4iIiIiIvBVc+H+vtI3r930Mzck5XD/AOJ0RaZBhVS2Cuzvnbv+V8VNKJSIiIiIibwXXP+A97d578LRfcRg1M5IkwRRRgklGpEWIb/hCcmf5PXc+fkLbyURERERE5FBd+cTcZHjv3jOea/5X8oRB2CNLE4zfaGCXy6SdXQo/JywG/2H7javPKZmIiIiIiBymwRuvP+d5+b/Eihn2B9T9BqV6HZPnOcZ1yKIe3bBFbaqE3949/b0nhh9RNhEREREROQyvvLP38Xp395TrGzrJkP5oROBVydMUUxQFdPoEvsPGzgoba3epu+afd65e+5lbH5mbVD4RERERETlIN/7W3GT/2iufLGXhL3a3N9jaXicol6DdhSzD5FEEOy2qns+xo3O01x+wd+sawaj7bLGx/pgSioiIiIjIQUo3l95Fd/eZ7r27bN29w9z8UWplj2hzjTiOMWG/T+v2Hbpbu1hRxrGzZ7Bc8IOC/r3bP6GEIiIiIiJykPbu3n5v4HtYfsDRsxcp5TDa22b17lXSOMb4nscoy9jb6TBcb1G1HMoL0+wlPXZW7j9z/eMLTWUUEREREZGDcPcTM7Od+w+e7mcp1SPzlKtTDDba7Czfo/Bj8qLAOEHA9EMPEw0zvMgQtto4NZ+Fpx6m09271HnldW0qExERERGRA7F55dpzWWf02Nz582QTs7S3OiSdEJNGzJyZpVQuYfB9vKkZTpw6j8kdwKbV3iW1C8499jDxg5VLSikiIiIiIgchvvfg6dOnzuFUG6xtbkMB5aDC7LEFqkcmcEslTB6G0B1SzE+SPHqC0Hj4vYDBZkhWrVE0vP6fP7T9C8opIiIiIiL76Uvv7HyyKDfvBzTJbu1QiiMG7gj7whz23DmKwTTJKMQBSG/dJhv2KJ0Yx5mfhjynnYwIFhcp3NYvDDe6/+zqhyYXH/69nSWlFRERERGRH7Qbzx6b3X715Ul7YfIfJrM19trgmIKJ3FB0hgyXW8StLkWWYxzfx6mXSNeX2fz2y6zevUFmYkrlgLjV5omXGtbUk0/8um1XOkorIiIiIiL7wfWc/tzD53//nS/XrVGRYVk2lcwwePMuOy9fJu61MDUXyzY4ySiCmRrlICJf3mPz7h5vbm8wN7ZArTrLzWfnLp790vY1ZRURERERkf1y8nfu9oH+9Z+tX+xdfoVSt0fr3h2yZIRdK+E0XIKpSfqA8byAbnuNpfYypVqJhakZZvwa3kqH4sYab3z7O5/+g5+fXFRWERERERHZT3/6san5u9/8zqf95R3S5Q2qc1M0zy5gNUu0lx+Q3lvBygtMFkWkcUyaZVhpihP4WLbB2AZ8j+rExI1Gub6qpCIiIiIisp9+7Le2Vxvj43cK28avVojTjNQCY2yKvCBNMwrAmMBjfOIo88EUsWWzsrvDdjaChxawzs5w4uL5L//IL91NlVRERERERPbb9FMPfyE/dxQzN0U+GJGutSnaQ/zpacyJWTzfw8mSiGK5RW58Rs0y0/MT1I/O0R3FmIkFTv1Oa+n2T08snv7PLW0mExERERGRfXH3I6cDos7Eyd/cWn3z507SWb5L+fTbqWy36SxtMBolpA92SeIYUxQFRW4TnD1P8OhFvMkjpP2cHEPuO7z09laxd+vW+1/98ITexYiIiIiIyL7oJ6Pqgzff/JmX3tEvvG6Mb/tgFQybFdyHzjDz8KNkqSGJE4yxLMz5sxRz03R7A/aWN4jWWhQ7XaK7D4hae7RHw8nHv6CTGBERERER2R+P/u7qzmrFSqPV7X9l3dqgfr9NeGeT7l6fdq9DNBbQeOI8XuBjMscB35DcuMdCK6JR5HRKQ7zJAC9KyLeHn517x4/8P5VVRERERET200dfnvjlpLX5WFRsEk9F5DVDqWcxH9ZIb96hyDsYy8LkSUy6vkZrY5Xt1jq5DY2ghh3mPLi7hL+48NLFz90LlVRERERERPZb/fyZP1y7e58ksmiU6ni+zcr6Pba21uitrxMnCcZkWdG6eRO8gspMk+rMBMEgZuN7b2Kwv9p84tHPK6WIiIiIiByExsULX87LlRe71+7gtkPKE3VK01VyHzbu3afIM0xujFUKPI4cO0L16DTtnS0G2x1qXp3m5My1C59daSuliIiIiIgchLP/aWu1NLPwkkeJbLPFcGud8VPzzJ5apOy6xFGMcYMS9dOn8SebtPsdWnsd4m5INkqpnjz9dWUUEREREZGD1Dx5/itZ4RENQrY319jeWqU6Nc6RU+dwHReTJAk0Gux2Oux1+kxMzjJ+9gLp2MRn3COzV5RQREREREQOkjN79MWi2vyVyumzjC8eYzgasb7ZwjQmKVXKOEGlQjE5RrWo4rpVavUxOtt93KPzL535rbUdJRQRERERkYN0/jP30m8/ceKFKOvfqFTG/o2dVbC9KpZThyDAFHkG/QHDKKfkjRMOYFApf6p+6fHPKp+IiIiIiByGd7xS/vymkzmhE2Aii3hoEUUFRZpiijQlygpSv4qT+Tip+9HJ02f+8MJ/WNJaZREREREROTSTj138/bw//CeBU8W1a/QS/scQ49kYLMp2QG7xQWdx/sXzn9/cUDIRERERETlMj3xm9075xMnnU8/7ROq5VIxNXuSYPjFmFBHvdUnOzF0++V92lpRLRERERETeCk7/fvSSOXPsxTTpYPc6pBSYJMnod3fJA5vu1vb57/3tY1WlEhERERGRt4Jrf+940NreOd9PYnqdHfI8x/GDEs7sJO5Uk7BU/pNwa+cp4LJyiYiIiIjIYRuubTxWHmv+Qd0rMSgsjG1jHMfBm55g6BYQDUl224uv/PTEonKJiIiIiMhhuvzBsVPhdutUPhwwNDn+1BiO62CyJMEmJ41H+J5HHia/H3f6s0omIiIiIiKHKY6iphkMP1cyNkWWYgzfP4nJkpQoGlHxHOw0Zfv6TQJjp0omIiIiIiKHyVCwe38JK0pw8ozCssizHMf2XIKgRJ7nDJaXGStVIEn/dx/33/2n71FJERERERH5/+v/ytxw8l88///zt9zkjm8V5Ftb+ONNChywLBzbcRhsbbEX5XiUyOIYr1LegFj1RURERETk0DiOSfMsJwljdlfXcb0S5DlOblmYKKZplSj5FexSQhZHWrMsIiIiIiKHqiANytUSTuDjdfq4eUpCgeNYNpXJadJqjeGDTXphh5LJAiUTEREREZHDlKcWYT+m3w+pHzmCFwS0LRuTD0aklsWg7OGdPMpOMiCKR00lExERERGRw5TEBcPMonLiNMV4k1EWYQowWOAYhyLMsG2fqfMXSY3RdjIRERERETlUuWsxffYMfjkgHg4oeT45YCwoKGwMAYPEwitVcccnbiuZiIiIiIgcJsfz216z+ePd0QjKVaIMijTBDDttoo0tSm4JOygf9yYmzluN2qqSiYiIiIjIYXr6y71rme+38YPz5Tyju7ZKFMU4gedZo06X4fIK5Xrz+KU/zV6AtoqJiIiIiMihu/TV0eXv/KX++0ejLnm/B3mOKVyP2swU9cCjOaVrZCIiIiIi8tYyeWTmasVzmJieICiXMMZ1sdKQigf9pXvPXP/ZqXllEhERERGRt4IbH5pc7Ny8+f6KazDJkDTNMFmvh4kGROEeJo0+17939z1X//pRfexSREREREQO1fW/vlgdLK1cqhr7lxj1yMMBSRRhTKVCXgnIRl2Cko0Dv9m9v/yMkomIiIiIyGHavfXgJ7ws/4JtWUT9DqbsUR4bx1SMTzI5zUpRkO/1qY1GBP3lJ//0/eFzyiYiIiIiIofhyju7n6x17j7pF7uMnBGbYYpVmaYoQkzW65N1hvhuieFui/7GMrUi+uedG69/+Obfm51VPhEREREROUg3/u58c3jr2rOVYvCPh+1NNteWqNSb2J0Y4hBTUBCs7LDo1gimm2zurLBze4naav/Z/r31x5RQREREREQOUryy8VjYar9/5/ZddlZWmKg2GDcuebtPkqaYKI4Yvn6d5MEqTslm5qmHKds2tfaQ6I3771ZCERERERE5SP2rt36yllt4nsPs+TPUalWS1h6dV14jHA4xdgGBbehsbbKxvkq1XqZyZAqDIX7j9nuvf3RaK5dFRERERORA3Pn5hWbvypsfDHKozh+hVPbYWVums7MFJseyrO9/J8acOUrXiiFM2Fldo2iUGXvsIn6WPzl65c4HlVJERERERA5C7+rdHwty61xw8iTUquy22wyTiHbUo35ukVK5jLFdD3N8gYmTxyjbLp7xWNrZIq+XGD9+gvj1G9pSJiIiIiIiB2LvyrWPzp4+jamW2NzbhcLCYJhZWMA+PoftOJg8yyg6HZoLR6lPz5DHGVlh2NrcxJqdxjkyfeVbj/U+rpwiIiIiIrKfvvHk4CP+ePN2MT3OTthlWGQkmWF27gS1uXmiJCTPcxzLWMQPNtndbTNz/jTNao1qlBJFOdZkE8s3v7DR7/6TK3/j7CSwo7QiIiIiIvKD9sonjlY737k825yb/IdMloiHMVNFg3rqQWJYe/MeUTaiWhSYnAIvsTGtPtevXGFzfR03MZSCGgPX4onv1q3ao2e+VhR2qrQiIiIiIrIfnvjN5X751Innq08+6vaqPtFYhcRxSFZ36P3FZcqdEbVSDdu2cUxuYy3Y+ElIeaPH8JVV2n4D/9hxvE2fu3/1bU+e/OLwMgy5+0/Pqa6IiIiIiOyLS38wuHL3Q3vn3Os3aHa75LeW6A+HxKUCq9FkasZjJ0sxtuuS97vE7R5lHGbmjuE1miQrD+itL3P923/xi9/92MyskoqIiIiIyH565aPHg9cuv/Tp1dUHZOubBLUGwewR7KDCYHePeLcLRYGTpjHJICdqxwR+GUo1hnFClnqkVZ9gor7iVoO2koqIiIiIyH5yfJO6jepmEZcI84LClAhwcPIMazhgtBMBYGzHwp+cZ2xsgSRz2N7YZjDo4xybo7Ywx+Ljj3zxsX+/FCqpiIiIiIjsp0c/czc9f+ntv9KYniSYnqA/6LK7vc1gGNNozNE8cprCsnCyOCFc36RnG7zZI9QmxqiNNwmTIfaxI1iT09de/fnR7OP/cXNDWUVEREREZL+88tEjk6f+w9rO9Q+do1hZYXL6KKO9Ab3tFp29HbyVTawCTJGk9KIRpbOnaV44R3l6kpFnyAMfCovulaujtcuvf+SlD883lVVERERERPbD5b92vNq6euMD33u6X+QZJK5LHjgUjTLjj55n6sIFdkcDonCEMZ7H5ENnaR6bJhp12dreYG97h2hrh/z+Er3VDazCpJe+sNpWWhERERER2Q9P/s79PrnF7uoG5t4K1m6X7c0t+vGIfrtF5cgERx9/FD8IMMb3sColdu/epB+HJHFKNU5ojE+QjiK6ve4Xjz3xts8oq4iIiIiI7Ke5Ry58KRyNPmNZhooXYEYR4aCP5Tjs3blFlsZ4vo/Jwph0bZX+xgaj+w+oWzaVoEwxHLKxvEzj2PxLF39zua+kIiIiIiKyny5+fqU9fmTm2vqbb5DECWONJkEBrTt3GG2sw9YmcZxg8iQq9u7cx05SJoOAZmMMK07Zu3mHNE2uTZ489bxyioiIiIjIQZi8+NDvD0ejG7t371BEEdNjk0yXy5QKi86dJaJRiMHCGgU+/qkTVI8fJRvsEm6tE2UF1cnjL5z/nf4VpRQRERERkYNw5nM7S+7p01/r5xHx9gppe4Pq7CSVU2fYrjdJUhuD7XD07FnGxycIwwGb9+/STWNS12X8/PmvKKOIiIiIiBykmXPnv2KXq4QZLN+6SzoKcUoBx8+fp1Qq/Y+H/fUGnVab9m4L58gsE6dPMyx7X/XmZq4qoYiIiIiIHCRnZu7yyHhf9WfmmDh1jlarzSBM8UoVHMfBGM+FsQlKXonmzBzTZ86ylWVUzpz5wzO/tbWqhCIiIiIicpAe+o+r7clTF74UOQH1+WPUpudwPB9rYgpTKmEKLNjdw3V9csejHSaEterfn3j08c8qn4iIiIiIHIbGQ49+vm97/yCyXXK/hHE94r1diqLAJIMBjCIyx8OqNRhY5lPT5y5+8ZFf0VplERERERE5HOd/YymcfPixz/fi7MOmXKZwHOwkowhDjOO6pI4N5QoZ1ocbswsvPfYbmxvKJiIiIiIih+mRz21sVBeOvljY9gftkoflABSYIklI4hHtfhdvYurao7/bvaZcIiIiIiLyVvDQ72yvuuMTd/rtPdI0oSjAmALi0ZBRGrPX7i1875MnHaUSEREREZG3git/64TT7ewdz7KEsL0LRYETk+FV64wvnqYw/h+FK+tPAZeVS0REREREDtve6tqTE5NTf2BKHnkKuQUmm6zjnzxLRBk/TJjudOe/+9Pe08olIiIiIiKH6cpfqT7WCEeTebtH5DWwTz1E4XsY23GIkoh81MX1De1B/8t5FFeVTEREREREDlMvCpvJoP8HQcnDykLiUR+LAmMBWRbT9CyKaMTma6+QJlFFyURERERE5DAZ26R3vvcdzLBHOujh2QXGsr7/sUvPtYmTEeHaMo1yiWrgDZRMREREREQOk2UVNCtlinaLqskwZORFgZMmMUlrm+0koWF7WBYUaVIBT9VEREREROTQeK4T2hZEwwH9YQ+7UqPAwlh5AaOImh9Q8UuUqlVs246UTEREREREDpNVQKlcwanVyJIMO4oxgAMFzswcuB7DlTWivQ4144RQqJqIiIiIiByaMEqDbm+At71L9dhxqli0jIVJ4xi3sPDqE5RPnKbbD0kzBRMRERERkcPlGTsdDBOqx05CfYyR8cCyMJaxKDKLUVzg2D7T5x8mS3NHyURERERE5DCFSeYcu/gwTrVBOEwoCpsizzAlyyVxLSo+DJ0C78jUByO3sqNkIiIiIiJymEp+ecdvjr9vOBxQqznYpRyTW5h4u429ukxQ9DE+DBcmbzz9B8PLSiYiIiIiIofpya92blBrrATNGm6yR3/lDbIwwvilEuFwyNrSCnl3+ON+rbaqXCIiIiIi8lYQNGqryU7rgzvr21ijhDzLMJbr4tabjJdqlKu1jcc+s9xXKhEREREReSu48Fur7eDI3GXPeFTqE5QqFUyWJLhFgYNDvNM6dfWvTywqlYiIiIiIvBW8+bGZ2WR5+enA8fBymzSJMVZRUEQRo0EfN8u+3L9//5mrP7/QVC4RERERETlM1//mYtC6ffMn7SL7QpGmRKOQLMsxdhBgNRrYVk4y6FNK09/sXn39OSUTEREREZHDtPPG1edqRf7rVpYSD0e41TJ+vY5xSyWKWpmcFBOOqBmDt7tz7ts/NvyIsomIiIiIyGH43l/O3m+2ty/Wsoxi0CeNRzjVChQFJktT6HcZRT1G25vE6xs0Xf8XezduvffGp49VlU9ERERERA5a++rrH5kw1j+MWztEO9vkeUI66kESY4pRSBGF1OdnKddrZDduEN67R7a39/Fwc/Oi8omIiIiIyEG69qHqY1ZvMJEuLTG6e5+S7zM2f4R+NCQchZhBq8XOdy/Dg1W8WoPSY4+AbfD7Azo3b/6kEoqIiIiIyEEavXLlI9bW9nszG+oXTuFMT8DKGtFLrzIaDDCu5xcVx6V9/wHDjVWyShl/YY40iei8ee3Dtz81N6mMIiIiIiJyEO7+3HxzcPPm+30D5WNzWBN1tleW6K2sUBvFeK6HcUsly5s7Qtc2dLKEqN3ClDxmL54jz5NzOzdv/YRSioiIiIjIQejcvvtjxhQXx8+fIQlc1nY26RUpu1aBc/I4pXIZgzEER48yfnyR0HMJSx6b7R0qczMsHF/k3re/9YtKKSIiIiIiB+H+t779CxPzszjjNXrDLoljyMsBtYV5/LkjWMbCWJYFYczM5BxHZucYRSMSUjYf3KY8VmPu6JHL3/ihgdYti4iIiIjIvvqLd+bvP3J87nIw3WSwu8FoNCRPMybq48zOHQXPJ0lTnHg4ILxxh7TTpXpskfkjx8jTAaOwCxNjBE74t7f6naXLH7sYAKHSioiIiIjIftjrt06N1/1fzGcaFPmAOb8CpTq25dK5cx+zsgV58f2PXTp+icHaBlvfe43R+hbGcnCMi+s5TD75+ELj5LEXn/yt+xpgRERERERk39RPHn1p5t3vGktMQeD52EGFcGmVze9+j/DBKqVKBcsYHCwLp1qhQcL2Tov2zha1qXEaR8Yp8hjbqT7+7v9qvqqkIiIiIiKyn37ky+FLtz7WuThaW8cZhbS/d4N8mGIDtXoVZ6JOQYGTxQlF1KcfdfFLHtVqDcdA7/4KA9eQ7CXPvfpzP3H58f+0saGsIiIiIiKyX17/+RPOrb/4409mD+4wmRY0SlUwKaPRgFEyoDRoAWC8UpkkHdLOBqROgVXyMbbBLwz1wqPhlnes3KRKKiIiIiIi++mR/3gvNUnBVKWJm2R4WDiVMibwiEzMIOlBUWDyIsMbP8JYeRp7mBMvbbO5PSA7c57OhXPU3v2uf/vY59Z2lFRERERERPbbsfc88/9onzrF8Ox5locx3bUWDGKqVo3m5HGM4+DYlkV/r0OERXVqgkpzgunpI4SugdlpHvnM1uq1v7nQvPhrK20lFRERERGR/fLG3zxWfehXH+y8/KGTuGaVhekjxCtrDLa3GMYhnd3/cRIz3N1ltL2FPTVO/ZGzFDM10mKEa8EENld/3CrWr1x79tsfapxTVhERERER2Q+v/fXp+eWXL3/ytb+UFROFi2sZorBPfqSB//ApaifmaW1uMOj3MX6txuSZ88ycOskgHrKTDdiLu4x2tiiu3SC9fR8vzoJ3/F7nhtKKiIiIiMh+ePS3t1ZLjhuO7izDjTuY3T3a/Tat/i6JFVOfm+HkU2+nVKlg7FIJa2KG7toWcZ7RSULCPKZcq5EmGd3t3c+Pnz+rFcsiIiIiIrKvJi+c/lrY738m7Q/x/QCyhMhkDJOY7tYO2D6u52PiQR/W1hmtbNNZ2aCcWzRsD5NnbN65RWlu9srDX9hZUlIREREREdlPD/3WzlJ5duLGzoP7UGTUyiW8OKe9usFwvUXyYIMkCjFZGJEsr5CHMVXLZbY2jp/A9r37hJb1Uu2h819WThEREREROQj1hy/+fqvIXtq8fw+TwOTkLGPGw4oyRuvrDPt9TJ6mxSBLmDh5jMn5Y7j9mOHmDrt7bYKjCy+d/7LewoiIiIiIyME498XdO97RhZe6YUh/fZOg1WV24ThTJ47TNzl5UWDswLcqp4/iLkyRdLvs3byDNYooeT7ByRMvKKOIiIiIiByk5unTX89dF0YJnXvLZO0upl5m7OJJXM/DlMYnYH6WjfYW/WGP2sJRavPHKPzyV5vzc5eVUEREREREDlL1xPEXMuN+rT5/DHdiiuHODp2wR9oIqFSrmJ6X4dQblCljj09i5mZp18tw8fTXz/62HvSLiIiIiMjBuvjrD/qVh0/9abvqUT55BmfuOKaoUa0vkgcuJs9yss4uXikA2yF3fBI7+LtTDz3+WeUTEREREZHDcOTi45+1gtrfDeMUHB+MBf0+eZphjONAGhFGA+oTk3SHyafqi2e++vB/XG0rnYiIiIiIHIbzv7axUTt5/sux5Xy0NDYOVk4ybGNRYCjA8Rxsx2aYph8N5hZfePj3WrpGJiIiIiIih+qh395a9eYXX+gPh59wKj5uySHPMgwWFFj4JR93YurGw/9l745yiYiIiIjIW2KQ+b3tVe/Y0ZcKcjLXpigKHOKEvbU1+pPT1IOpqjKJiIiIiMhbyWgwnNzdblEvEtI0/f5JTKUxztTp8/i+1//zH3efViYREREREXkr+M5PVh5zg6A9fu7C9x/4AybPc5ypWXKnQlqY7/q2nX77A2OnlEtERERERA7TSx+sPOYQV/Ph4GpRGJrHTn3/Y5eWsUi9MoPuECeoMBp0v03UW1AyERERERE5TGk0mB1tb3zTG29i5TCKM4yxMRQFeZbhBh5uHNO7eweTJ4GSiYiIiIjIYTJOQXt1BSca4WNheSUsy8JYGeTWCNfpkdx+k/FOj4rlhEomIiIiIiKHOsQYi1ISk9y+i50WuAlYOZgiz0ijmI2VdcI4pDAWuHaqZCIiIiIicpgc2+tjDEkcsbP8gN6gQ56mGNt1STs96m5Avd6EcpmiKFRMREREREQOVZoUWMZQnhzHL7tYaYRVFBjbD2gcmafslxj0B8RFTu7oOpmIiIiIiBwux3LCOMvZbO8R12uUxpsY18VQ5KTDEd74NMHsLN1wQDsaTSqZiIiIiIgcpjjNnV44ojY/R7VZx8ICy8LEwyG26zMcjLDKFepnTpMUmaNkIiIiIiJymNICxs+cxlTK5HmKlafkcYxxPJfEcvFtn8w41KoVilLQVjIRERERETlMnueGfq2MjYWTW+SW+/3rZL12m3inhZNbOLZNVq38eHN66oaSiYiIiIjIYXLGG0umWX9fbhucvCDf3mHU62FsINvrsLqzR9ztvi8oVzee+tW1HSUTEREREZHD9ORvrLaDqfHb3XD4vo2tbYqtXYos+/52svpYg0mTYsZrK4/9bveacomIiIiIyFvB2351986kH7THbBt3ZopSpYpxSmXcIqbsRISt9Sdf+fnpeaUSEREREZG3gjsf9S/Z95ef9m2buFECx8YQp2SjPmEUEuT8Znjr/jNKJSIiIiIih+3m3zsetO/ef4Zq+d9E4Yh0b48sjjGubTMKPGIrh1HEVOb2L789elbJRERERETkMPW/c/U5p7D+dRiPyPOUUmbhBgHGxabUbOIEHnl3iN3pf9ne2H7syo/aOpEREREREZFD8ebT9nv97fbxkhMwGPRxXUNQqVIUBQ5JTjEYsZeOGBsVUBRULPNP7732vePw6AvKJyIiIiIiB631ysufnJ4afy5s9xjFQ8gLylmGlWUYK0kw/RGTY+P4jsXO7TcJN5dJdzYvvvqB0pPKJyIiIiIiB+nGz1YeSzubF4frD9i6fRvXdqjXqhTRgDRNMflwyM7rb0BngNusM/b4GSIzoubaT7Zeu/qcEoqIiIiIyEG6/9Kf/2NTyi7mTsb8/DwzEzOk3T7rd6/T7XQwVl4U9cwwerBBb3cDa8wnmKmTZQOKO/efuf3xE44yioiIiIjIQbjz8flmvLn65MAOcWfr+GNNevce0FteIbRiKAoMxlh+bZy01WWn22K7tw5TZeYunMUeDS8NX7um0xgRERERETkQgxu3frLo7Z46enERM1Ul3N0haXVIh0PqE3VqjQbGrlawzp7EXzxKwx/DaxuKzQinMkb5xDyvvv6dTyuliIiIiIgchBvfeenTs8cuUA7mSbZHDOMUc2QGb/YozcWLOJ6Hya0CXEPj/Gm88UlwKvR7EeHOHmMzsxw7Ov/SN941+IhyioiIiIjIfnr+nb2PN4/OXW405xms9Ym7CW7Jx2nWGD97AduukyUJzrDTofvaNfrdKSYvHKNbOHhxBYY2NnXmh+YXW63BP7j2sTOTwI7SioiIiIjID9r1T510ll7884XJ2ZlfKColsq5FfSrAC3wwATu3bxKtb+MVBabcbOC5NtxdYvTy6zTubjCBIbYj2s2CM3dnrXhx+lpirFRpRURERERkP5z/9btpffHYi83HHy9FJRfHA7fu0+3tsn79NQYP7jDhOliWhZMmKcFkg7HdiOjuKqx6dO5vkJ6cJzQe3/zY1JM//Fujr8Eqd//pedUVEREREZF98c6vZS/ceq59Ll/fJI16bL52mySOsUYR041JgskGPQoMeUE+7NF1UnbKGaPpEk61hHd3G+ubN0ifv/LxNz4yO6ukIiIiIiKyny7//Hzz1suvfHLj1TfJbqzQNBWm7RJ+VpDmMdmoR5EXOEWek4yGDIsRwVSTolIhCy3cUcpM6hImDrpIJiIiIiIi+y0vCscUJi0bDy+JcQnAtSiXE8IoYtTvfP86mR34+LNzlDYfkO+MCJc72F4F98gCWXPqX1SfeOyzJ7+4saGkIiIiIiKyn97+G2s7d597+y9nvjeIN9b/Zbi+TGInZKUAXJ/S1CyJewPHKgrY7eP3CsYq0zDfgIUZOkSk5059/cLn9+4op4iIiIiIHISTX9xevfVTx18oKtB42wm6a6v0N7ax+jHRXkxRFDhRv8fo7hrVRy+QnWiyVS/omYSSVWXC+P2r73ee3F3bPVdbOPESoIFGRERERER+4F77qelTu6tLl6pzjaXBVHlnSJmtsE/lSJPZ8Uny1Q6DV2/S63Qwlu/jXzqL+9AR6Nn4dzLspYRqJ6F49bVXkzeufNfNw+rb/uuGBhgREREREdkXj/7B1p3UHjXzq7e/aV5647q93iFp9UlafRimePPzjL3rCVzfx/ilMqZZpre5QjqICGKPidilmRmCPKff2nx+7NTxF5RVRERERET208z5M18Nt1pfrMQ547jU44ysPyAMR4S9DlRLlMtlTBrH5FttelttWt1NjBsSOBlWlnLj+lXy6elr57/UuaGkIiIiIiKynx75rdaSNTd9ZeX+LexwSJCnNKse7cEuW2urZK0OWZpion6P4coWYZyR+TnOTAk/GzC8fxeKfGP80pOfUU4RERERETkI9bc9/IVhFl0Ll5ao5jmlRoWcmH4asXt/leFwiHFLAVacM3v0GONHpwnDXcLWBr2VFSbnj/3FI5/rX1FKERERERE5CI/93t6dytzMlbS9R7G6Dt02C2dPc+z4cUycYQoLY/yA0rHjeLUmlueytnSPNAzJ/RK1E+e+rIwiIiIiInKQxi889PuJ7WFlGWtLKwxHQ9xqicbJExjXxuRpipkYo98bsL26zvT0LKWTpxhVmi94C6e/roQiIiIiInKQ3LmFl9p++bK9cIrx+RNsr2zS7w9wJqr4pdL3r5MVjRqu7zPdmKQ5cYSu5eCefehLZ357c0MJRURERETkIJ377PZqcOHiFzulGtWxo8xNHcOzDEzWsFwXY9sOUTgkdW1cPIgNSbn+D+pPXfpl5RMRERERkcPQfPulX+749U+lRQmnCHD8gP6oDfD9FcvhaEBa8rEdnzQp/lF98cTzb/u1u6nSiYiIiIjIYbj4aw/61bPnv5Ik1s/Yfo3cthmmQ7IoxCQlm3rQwI89+vXahzunTrxw8XcH2kgmIiIiIiKH6qnPre/0FmqbnXLxYc8pMRU3sY2DyYocYwx5Ad7Y2J23/374knKJiIiIiMhbwTv+S/yiPzV+Jy1yCs8nTRKcIkkYtbbojk9SGrmzYFRKRERERETeMvrDwWzU71IfZqRp+v0Vy3bgUp6ZxK9WNr79V+1nlElERERERN4KvvOzwSUn8Nv1M6fIHAvLWBgsgzs/D5UyeRR912Rp8K2fKT+pXCIiIiIicpi+9aHmuTyOq25ufbPIc6rH5vE8D2P7PmGRY0ZDLM9n2Nr9oyKMqkomIiIiIiKHKY/iarS++SdeEGAlCakxGM/DGMfBWBYlv4QXp7Rv38WEUUPJRERERETkMBVJEnQerODEKYHjEKUZeZ5jHMeGNCOLY6z1DWqWQ9l2IiUTEREREZHD5Lpu6EUpo6Vl4jQnz3LyLMMp8gJ7FDIaRJjRAGcYYmFSyFVNREREREQOjR1njpvm5HFKf2WdoNrAosAJBwPsOMJKchzjUClXsNM00KplERERERE5TJ5t0slaEzdKMMYCawAFGAqwJipkcw0GVk43yUm8oK1kIiIiIiJymOIsrnajHrlT4M9NUZ+fwg4CjHFssjzHq9QpzR+lP4qIRrG2k4mIiIiIyKFKctjudgkWj2MFJbI0I0sSnCLLyAobywlw3IBgYRHjen2IVU1ERERERA6N5brh3LnzJJ6HQ06WRECB8YOAzHbJc0OKTaUxBn5pR8lEREREROQwOY7bzyplkjzDsx0S26bIC0wUhtjDIRkGbBt3Yvxxt17bVDIRERERETlMdr226U8032c7hmGaEPf6ZHGMIcsYrq7RabcZdts/XG7UVx/77ZW2komIiIiIyGF622+t7fiNxv00Dt/X2m3RWVnBsiyM5XmUKgHVQYf6WH3l4S+u6yqZiIiIiIi8JTzy250bQbW8M5amHKnVMcZ8f8WyXy7TKNt0Vu8/c+NTC02lEhERERGRt4Ibf+fIZLizc65eDihZFlYBJgpHFN09ssEeJZff3Lh/58eUSkRERERE3gqW7tz6ycJKPzfca8Fw+P3rZIHnkxofu/CxhiHZqHP8xR/P3q9cIiIiIiJymP77TxbvdbqDSdOLMdiEgY/lupjAuBTji8T2DF57xDEv+TfNW1efe+WneK+yiYiIiIjIYfizZ7uf9FZufOD4gH9T2UyJ8xLDxQUsz8Ek/QFm2Ke1t04/7RMvb8FE/eMbV65/QOlEREREROSgXf+FxWB45c576gmfjkYtunmHaNimttMmDyOMi8EJeywsTJCblPTWA3a21ii2ds7d/MDkohKKiIiIiMhBytd3T1WX2x+xt/ZY2V4hrKaMTddwd7qkcYJJhyE7N9+g392iVqtQWTyOHxeUo/g94e2behsjIiIiIiIHKr129z21YUI2GtJcmKQ6XqEYdOnduUG/1/3+xy7TdMTWveske12s6UnKVoDJEjaX77xHCUVERERE5CD1by89UxQ5jFWYmJ1msLXNzv37DEcdjG1jXD/AbdZIRyOinR5hp021Ps3k+ZPs7K49+fq7c53GiIiIiIjIgbj+pPXcaHn9XdULpzFzU/TWt0i3u2SjEGdmnFK5jLHKJSbOnGPx2CkwDrtFQdId4E80OH5iYfH+t1/8n5VSREREREQOwsZ3Xvr0kbOnZp3pMTqjIcNRQtWUmZydZ+LCOUqVCiazCkhzgrlj2JMzZLFNh5Rwb5dGo8LYfHPpz35cpzEiIiIiIrK/XvkJ92l/YfyyO1tntLdH1B2RWy55s0H51BmIIR6NcMJhn8G1G0RnT9M4cpQj3Yx+OcHJR2QmwwmGn0zXt27A7FeVVURERERE9stgfe3JcML7xcbUOPbuHkeqxwjdAlyP5e0tyjfWSJMUU6pUKQKP5VuvsXH3KnnWwfdtilYHrzLB1FNPL/izM1eUVERERERE9stf/I3jQTg/cefou3+k5DTHsKIIz7apj2LiN28S3biOF4DnGZwiSihPjzMfWXSWbrCzdpeyV6M2fRSzsge16eY7n7e+pqwiIiIiIrJf3vUb90MwX737c0U1vfmAvMjYuHaFUqeHGcUcnZ/Cny2zYyUYy3UwG3tU9hLKiU3Vq2Ebh97ONjduvMHN733v45d/fr6prCIiIiIisp9ufmwx2Pvai/984+ot1u8tYQKXpOrTs3N2i5i9cEieFziWBekopeh0mJidpB94UCkzaO0R2xZZEjWH/cEM0FZWERERERHZL2FnNN/t9I67lkUeeES1KiXbZSaostXaIktyfAucPE1xJqZI04yo12fYixhYUD1+kuPjM/+sduGRL5z6YvuGkoqIiIiIyH569L9u3bn703/pF/pvXL6Sh9v/fHflAUU/xE09Jks1ylOL7Fiv4xjHIUlGtOyM2CqYeeQRFsbHGfZHmMnpqxpgREREROT/w96/Pmma1/ed5/v6Xcf7fOc5szKrKquyTt3VRwpooQYhqW2BAMMMksCCMYxhB0fgCM3GOtaOGMfKMd6Y3VhFeDfkGMtj1sgjZoQHJCGBaFkgNVJLIKm6u7q7uru665RVlZXn0533+b7O1z7gwf4DVGY9+LweXg/fz75x/X7fn8hhOf2H2+tXf2H+ctDJWDoxTaUd0vu7t4h6Q0oHHWzHxgnbHXr7O/in55mam8JqVhkNBri1Mu7MzLW3fnH47ObWzsXKseM/Aq4pq4iIiIiI/KR977+Znu+9c/OjT03PX7aa/u0w3iHJY7JKiep7n6K/u8vy6n2qYYjjeB6Ns2ewzp9g2OvQXVvDtm0sM6IWL1/faw0xtebnf/r53rU7lxRXRERERER+8j70v++sv/C4nbbfvvV6UDWYaspef58osxirNKk9dYHoWIP45bcwTrmMPTFJd2uf4c4BfuFSzl0m61Mk/Yhut/uV5uLxy8oqIiIiIiIP0sSpky8e7La+bhUO9dxh0q2SD2JGnQHb6+vUm2OUymVMFobEOy169zbxhgnVxKKSO5hhysHKKsb3+pf+QPdiRERERETkwXrqO63lolba2ry7jHE8gswwY9cJWiH+/Q7xaos0TTFpEpMu34HhiCwb4TZ9ogA66/cwg+jFM08881vKKSIiIiIih2H+mff8VjEMX0xu3cEnw8xU6Wc9BtGA/q37hIMEY2y7SMmZOn2cxsIcWeCyt3KHg91tqtPT1y78cWtZKUVERERE5DBc/E5reWxh/sr+1hbb9+6SpCETjywxfvoEtmtjjIfBNlbp9EnsyTEs22fvndtkUc7IdrDOLr6ojCIiIiIicpjcMydf7Jd80jhn//Z9iqSg1GhQPXkM27ExhWVwxur0k5C99S0qtQkmZ0+Q1prPe3PTWqksIiIiIiKH6/TCS71y8EJ1+hjlyjg7y/fpRkPc8Tq2bWNsP6CYmqQdDWlMzVIfP4ZVqlE5cfylx77Z1RAjIiIiIiKH6sn/tL1VPXPuT+zmBPX5k9SmjhEXOelkA9v3MUHgM+j3KNWb5LZL7pcZOsF/P/30u7+ifCIiIiIichSOP/XU1waW+89wfEzg4ZbL9IdDjGNjojDE649olBrgBwywPu0cm7/8xP+6taV0IiIiIiJyFB7/6uZedens8/08/6xVrWEcG2c4IhmNMMZ18KsN8jAlscznzez0tUt/GulxSxEREREROVJP/GHnRjw1sRw59mcLDPVqA4OFScMQRjFebgiOLVx+/Ns93YMREREREZGHwru/G17OpsaXXcfHhDkW4GRZSrGxQ5hCOj1RVSYREREREXmYRCYL0nYbd7tNEseYtOzSmx4jPHeBwvLb7zxXekaZRERERETkYXDlH7jPegm4pxfZnp0idQymsMHMz5GWPMpJetuPs+pLHws0yIiIiIiIyJF67ePVJ00UNct5/peFsanNTuP4HqbIwXIM5XiAMTnRwd6fW0USKJmIiIiIiBylNI6ryXDw3cJxMKMIx3OxLQtjux4kIa6VYkho3buFlSe6GyMiIiIiIkc7xBS5s3rtGiaKCXyfUbdHARjj2Lh5BlZO9/4dGPXwbJMqmYiIiIiIHCXbdcJykTPa3CALh5RtFwCTxjFFnnFw/z6FZWMcD7vIHCUTEREREZGjZAG+4+MXFp2VFfIoosDCpFFEsrePW1iUMPjVKj/eviwiIiIiInKEQ0wBrusCFh4uSa8LRY6xHZfq7DxWc4owy0nTnAwdJxMRERERkaNVFFaaOQ5hXpA1mtRmjwEWxnGdIrIcrNoY1bkFWrsHjMLUVzIRERERETlKgygNdnf2KM3OUZuZIYl//K/FFHlhxTkYyyUxHhOPXMSyTKZkIiIiIiJylPzA70+eWsL4AVaekzkuWBam5AVYSYJrLKhVGExVcCrlPSUTEREREZGjlLpJ4M/OktglsAKszMbJwSRhSClLGIV9iiLHazZ+1pSqW0omIiIiIiJHqVYbX/ZK5Z/ybIsoGkE4II0iTDwc0lu9SzjskHc7P1uvNO696//YW1EyERERERE5Sk/9b5t7TqWx1mu3/14cD2lv3CVJYozj+ZTGqrjtXcrlUvuxP9jXACMiIiIiIg/HIPP7O+vlRuNesrNGtebj+wHG2A6Oa1Mv+4StnYvKJCIiIiIiD5N0f/ditRpQrvo4nodJkgQ7jPFdGztLgysfSD+uTCIiIiIi8jB45YPmOTtPgsC1yQZ9knCEAcgo6OzuEdj2V51Rd+HVnys+pFwiIiIiInKUXvpA/lHT219yXfONYbdHWOQYz8ME1Qr2zDzloELeOaCaR/9udP/mR177B+Y5ZRMRERERkaPwxkecD+ab936mauf/odjbxrYsqtPzuKUyJgpH0BuQJDntrU3IYsZr7q9tvf63v3b7n8xNKp+IiIiIiBy2rasvfbnqJP/cSYZ0d3fIsoxiGBH3+xiT5DAaUiqVCLyA7evX6a2s0N9c+3jY3n5S+URERERE5DC98Q9Ll4Ybq8+Em5tsvf0mtmNTqtbIeh3yNMU47T4Ht28SxkMqE9NMN+ao5gHjQZ3d+7d+QQlFREREROQw9e/d/OiME5wsJYbyzDz1E/MUww7JW9cY9vuYzLHJ20M6y+sMWi3M0nGymk8+HBHc3b146ws6UiYiIiIiIofjzudPO+V3di5aYUp3LKCyNM/+7g7D5Q2KToyxbYzt+zSrY2SdEfv9Hu1RB2dqnGPHT9FbXv1oenfjklKKiIiIiMihWN24tLFy51O1c8eZmj9Gfr+Fuxsy7A8ZzY/hBQHGdV3spSWmTx7Hr1bodnsMRyNKs7M0x8fZuPLyl1VSREREREQOw82XfvjPx86doDQ1TtzuU3Rj/MSienyO8ceWcBwHkyYJVFxKi3NUqlWKMKUYRkR7m5TPHKdoeP0f/H2tWxYRERERkQfrh7/gfNA0g/bUsXnC3TaDbp/9IsKaH6N5egHHK5HnOU4+iti98w7FmRNMj81QdStgUobFgLgRkJbczwxbW1dg+gVlFRERERGRB2Wts3Px5PjYF8DFy328xVnW/JRRs8poZQ1zbZ0sSXGCSplhHLJ75waW32a8PIE9M4EVh5Rnp1mcOLawenv7qSufORUAodKKiIiIiMiDMFapbjWeOr9g1nfW6HbB+FjDAfd3NjBbW5yxXbLAx8RFwYnZeU7iEu9ss3HzbXbefBOGCYN37uEN0uDvf888f+nrdzXAiIiIiIjIA/OhH3jfypMs2N9eJyk5HLz8Kubl65TfWWPMsiidmgALTIEFrS6VgyFOnmHGqgytlP2VNUY3N9h54aUv3/j8YqCkIiIiIiLyIN36h3OT8feufCG6epfN5Tvsezm55zCWWTR7IfnuDnEU42RRxGhljdhklE8uwNgYfu4Rru8yHCVsup3FdNSZB5aVVUREREREHpSugf3d/QuVYUJlrMzU7BTxYIjfbtHfXyXtHZBlGY5fDiiff4S8vUV32Ga0dp+prsE+/QjjF+b+pf/uc89f+saBBhgREREREXmgLn19c+/6Z577H5LXXr9ubR/8Dwd/c4U88AgrLkWjzuTxOdKrP8IpgHw4ZDAYMSjB+KXHqDYmyHJDPjZ5/dI3+leVU0REREREDsOFr3duXP3Z5ordcJl59ATtzS327q5QTjOibojrOjjpaES706I4PseZ48fIG2U62RBj+9Tmpq7d+Fh8aW9l68nSwvHLwDVlFRERERGRn7TLvzh7Mdq89+T04vgNU/GuJev3GFk59dMnCcaa7G3eZ3tjF3cwxLiuR/X8EpXHHyGzXfp31+hvt4gPuiRvvn09ffvWK75tp+/6L10NMCIiIiIi8kA881+2rg3tjO2b11/h5t0fZns90o1dDlY2sDyP2UceZezCWSzLwhRFgTM1TdiPGG21iboRY4XLhF/GymB/d/9r9uTEDWUVEREREZEHqX7i+OX9fu8rWRhTtTxKMRSdEYOdNnk3ojw2het5GIxFeG+T7uu38QYZE46P67hgYGtzDVMt7z3959FlJRURERERkQfpp/+wtYxlsbG5DuUSluMy1RjHPhiwf/UW1t1d8jzHFORwc5Wpbk7c2sdemKGoBixv3GczHlxuPvP015RTREREREQOw9J73vtbrWR05f7GHcxklbzsYEYx1V5EdHOVOAwxxrJy20BlYZra0+dJ0wGdjQ2i1S0alfr6Y3+k7WQiIiIiInI4nvz9/tVytb6+t7HF5t27GCunfmYR/8xx4qYPxmDiUWTsxTnSJ+bZq2Zs7W3i3VrnTOxzZvbM95RRREREREQO0+Lc6RfOOQ0qW10GK2ukFZvoVIPh2TE818Xk9RLO/DRRd0S4tk8RQf3MGbZK/mVzZvFFJRQRERERkcNkLZ56cde3f1Q9OUdmw+7du7iZQ705R2YsTOhZ0KhijRLqpsbxpcfpVeuYxy5858w329pKJiIiIiIih+rR3+9ftc4ufX/QKFM/cZypuQXCvT7lyiR2OcD4QYk0TYjSEcFYjTgtMJXmP5k6+/jvKp+IiIiIiByF2QtP/G7u1/9FZnwKY8hMRhYPsG0bM9jdw2rvEYw3CLOc0PE/a8+devHif95fUToRERERETkKj/7uwXIwd/b52K9+GiegOTZG0mkR9vsYr1LBjNUJ0xGF53/RHpu+9vh3dIxMRERERESO1uPf6V0zzelrwyT7p6MkojRWxXZdjBOUyJOYcqOBW2+uPP4nWqksIiIiIiIPh4vf7V7zZ+euuKWAvMgpigInjyL2N7YYJRaTc5N9iFRKREREREQeGomx026nj9VtYyUJxjIW5UqDxrFFPL/UfunD9YvKJCIiIiIiD4PLv1i/aBs7LJ8+hyk3yLMMgwXu5By2XaLI8utelgavfXz6pHKJiIiIiMhRev2TkycDA1aWv+Ul0Jw6huN5GKdcogiqZGFB4ZUY9ruvMOpNKpmIiIiIiBylaNhd6G5tvIXrkSQFeVDFCwJM3BtSRF28RoYz2CK//Q5x0ZtXMhEREREROUopuRNvreAnXXw3h2QEcYrBWJR8BzvsMTzYxh4NMaZQMREREREROVIlx+0X4ZD+2n1MGuPaQF5gyHPS/ojW5i4myujstzGuP1AyERERERE5SkmSBcP+EM/YtNfXKIYhljGYJAoZtdq4aYGVW8zMLuAWJlUyERERERE5SsZ1wonJSZzc4OUF3d29H78TE1Sr1I4dI0xzknYP4hRHp8lEREREROSIWcYOwySmMEBljGalSg8wluMQ5QWV8UnKE5P0en1GaRoomYiIiIiIHKUsz4JOf4A9NYVTqVJgsFwHUxQFCRZZAUW5QjA3h+25fSUTEREREZGjlGdpMD4/T+56uIFPXhSQFzg2YDsOlu9hO9A8NktHvURERERE5IjZjh1W5mbJPAeTg+25kGWYqNvDSmK6RY5lLJxS8DG/XN5TMhEREREROUpOpbxnBd7HijhkGMeEwxF5kmAoClrr63RHQwbx6GcJgvYTv9e5oWQiIiIiInKU3vV/7K/4gd9PsuxjSRyzu7ZKmmWYml9jop9SvbOKU61snf5B/CPlEhERERGRh8Hj30lfDGrVLbN2j/k4xPM8TIGFPztDrVYl2t07r0wiIiIiIvIwiXb3lxrNBv5EkzzPMUUUUcQD7MAjH4bNyz+ff1SZRERERETkYfDmz1nPMRhN4tkkTkaeZxiTZgxHPYZRn7pf+h3/oDfz1nPus8olIiIiIiJHOsB80HrO7LaW6kH53/VGA3bDHrbnY+xSmeDMGYxrkW5tU8d8Nbx357nXP1l9UtlERERERORIBphPVJ7MNzYvlYz1Hzg4oKCgcfoUdinAEEYUu3uk4Yi4vQ/xiHrg/Y+bV1//nNKJiIiIiMhR2L/x9iddu/h/kcb09nex0pii1SHu9jB5lmKFEY1KlSKLObj9Np3Nu8Sba8+8+Sv6GyMiIiIiIofrjc9Oze8v3/5Qf2eN7eVrpGQ0/Ap2Z0CeZZg8DBneuU+2e0Dj9AlKU1XcIqGSJM92bt15TglFREREROQwpddufnTcWM/k+YDywjhjJ2YoekPC6yvkaYpJo5i81SHe2Sfa2SRYmAHfYMcjiuUVXfAXEREREZFD1b9z74N+EmPVXconJtnfWiXcaWHt9kjTFOOWS0Wl3qDTG7B/sE/UOaA2M8Xswjzdu6s//dZHGno7RkREREREDsW1v1++1Flde6Y5M0NjcoJua49eFNI7OCCoN7CMwVjVkmU98SjJyQWSyWk6o5iwNcAfn2P+xPHZtR+++OtKKSIiIiIih2H5rVe+1Fg8sRTMn6bYzXC7Dp5XJ11cwHvfY1QaHoYCLAfmTh5j3HIJOhGkKb3hLsljc7Sn3P5ffMTTsTIREREREXmg/uwTzgfx7HBmdoZkewcrTem325QrZWZPnyQNDEkU4Qx7XczVW3hnFglmJ+iOG0LHhjDBczyO+40vDfb2LkP9R8oqIiIiIiIPire+v+Q0qr82bLqENZtSfRbaXZqNaZK7W1irO2RphvErlSLp9+hev0nr1i2yOKJcr2HlGaW5OeaeenKxEvjtH/2j2VllFRERERGRB+HqL0+eLPl+f/7JJxa92WksB/BdnCLn4OYNotVVTBjjeh5OUWDZ506S72/D1h7p+gat+iZT587QWb5N89Iz6bN/5X8Ltrjz6xdVV0REREREfuKe/P29FQhW7lwImoNX7zBRQPvNqwy6bQLfIzUeldMnsF9zMbbtkI56ROmANBxRdcpYWx0OXr1DdGuTzb9+6cuvfF5/YURERERE5MF6/Ytzk+t/+/KXh3c2OXhzmfz+DuOFB6OUPE/J4iFZEuMUVkG+skFejBg/s8iwWsW3qrDaxqy12OltPBM8GX4N2FJWERERERF5UIY73fnNze2LfrfD1LFJgsdOkw/6VHt9Oqsb9DurUIBjFRbBhVPku2t0tnaJ26vkuNROn8H7maf/+2MnFi4//XvtG0oqIiIiIiIP0k//8eDqrY+9/zeSm3euJOtr/6b3l5dJ3ZxgZoJ0dozazCwHb76Dg7FgmFC3XNpFQv3iaYK5eSy3QlRtLL/r+eiycoqIiIiIyGE4+93B1WsfHFtyxzxmzk2xv7VGa2sHN03JoxzLWDjJaMRgfR1rZoz5M49QTI0ROgUUFo3J5r07H0sv3dprLbnH5y8DK8oqIiIiIiI/aX/9S1Pzg9XVZxfnZq6asnWtvXEL0yxRnzhDbWae0d0Nuve2iEYxxhQWXDhB5b1P4VZmSe6FdFcPCFvbJG+//Fb4zsuvVAedhZ//vX0NMCIiIiIi8kB84A9214Ni1HReefO6c+Wd63a7S6fV4mBzF98tM/bU07jnTpPnDsZUylQmp4m3NxnurdHt7dLEMG4qWH6Zzdb+i/7x+SvKKiIiIiIiD1IwMXljp9v7GsamFpTxwphR64Bob4tRa5/69Dj18XEMeU6+22JvfZ1Ov0Wt7FDGwUltNt65Radc2n/3n6QvKqmIiIiIiDxIP/Wn2Ys9Q7p5Zxkns5mwy0x7JeJej4M71wkP9kiiCDNo7TO6uUw06BOmA+xGgHFchjfv0W+Pbpx8/wd+QzlFREREROQwzL7vvf+2PRxebt1dI7B8gkaDJByQZCHrN64RDgaYIAiKIjdMTc1ybHGeUtklXVsl2tglaE5fu/SNUNvJRERERETkUDz5p4Orpbm5q+31LXorGzi2y+TpRZpTE1RKAUVRYIznWd70LPWFJfygwu7tm4SdfYrxCepL555XRhEREREROUze8fnLbq1B3gvp3l+DUpn6zBz15gSO42CM72HmF4gj2L23wWgUYhaPcVB2v1U9d/5PlFBERERERA5T49HzfxI53rfc2Tn6hc3qxhax7VCeXsALAoxVgDMxTS9zqJSaLDz6OEzUGZ6Yun7xm1tbSigiIiIiIofp8f99a6s2P3fVr9WZPHOWvFKjm1lQn8JyHIxrG0Z5QeZX8Eo1TJ6zH/e/MvueJ76ufCIiIiIichSmn3ryd+Is+5eZ51KeniG2fYrMYAFmv93C6bZwiEn8gFap+UXv5ONff+Z/7V5TOhEREREROQpLv7+/Ep1a+l7b9n/JwqFiDFZnjygcYtypMVwPmoGhY6wvjiaOXXnfdzK9CyMiIiIiIkfqXX86upJVmytWymereQ5lh8w2GNt2wDYUtqE+MbbygW/3ryqXiIiIiIg8DH7m+dGVUrOxnleqkGWQF5gsjtlbX+Puxip54PWVSUREREREHiap4/VX1zfZa+1jGQtTpBmu7zJx/gJFnjuvfNx/RplERERERORhcPkf1C5iO2Hz3DnSckCe5xi3VMI7sYBjwMAPTZRUX/mViZPKJSIiIiIiR+mlXzk2SZwGrmW95VoF5fljOI6LSeIIy/VxigLPD0iG4Z8n7faikomIiIiIyFFK+r0ZEyevAJgiIzcWxrExtm2TRwlgY/WG7C/fxc4KR8lERERERORIZWmwfeMmXppijMHFoigKTBzHlB0HO7coekNKUYqfW6mKiYiIiIjIUbJti1Ick+ztUhQZrmth5QUmqFYp0oy9O3fI04w8TLAsoyFGRERERESOlGMV5OEQE0e07q+QxSGFZWHi0Yjhzg6u7RB3eniej+XYGmJERERERORImdykjaAKYUbJctnf3gMKHC8IisrMjDUobKxBSmEKMDpOJiIiIiIiR6uwTDqME+qeT+o7TNbH6RmDKYrCSm1DY7yJP9Fkv98lNzpOJiIiIiIiRyvKs2BYpDhjNaamJymAIs9xsjAisX68siz2XcyxWcJoNKlkIiIiIiJylIosc+xmnci1Ic+IHQcsC5NmGVmeMnIs8GyOL54gDdy+komIiIiIyFGyS25YPb5AWikxtC2sIsUCjE1BOU1J0hzHsYkD5+9Zvq8hRkREREREjpQJynv+RPOpIknJsxw3SkjiGJNnGcOVFawkZTAYfNgul/d++g+715RMRERERESO0nt/b2c9MxY5xU+5ucXB8l3yPMdYlgWOR7S5SbnRuPeeP+hfVS4REREREXkYPPPt/lXHttP+/TVcr4SxDAbbpjo1w9TYJJ2NrSff+dJpR6lERERERORhEbe78xPjE0yNT2MZC5PnGXR7GCysKPnGxr2VZ5VJREREREQeBt97zvlgOBg1fSyKUUReFJhaYmjlHq0IJgqY27n/7OUP9T+nXCIiIiIicpRe/kj4qfHhnZ+frmS/0+lEDJISnskxXrVGefYYVpxikpDAyv6n0erKsy99uvKksomIiIiIyFF481ca50drdz/YMMWv56M+nm3wpqdxAx8T9nv4/S75sEO/uwdFyoxf+VLrypufUToRERERETkKW2+8+Zmacb/sWDa93V3SQRu71yILQ0ye51iDHs3AkGYh26v3CO+s4t5eefb6L02eVD4RERERETlM7/zDhWb3zp3nendXObh5GyeJKVVd7KT348cuSdOi+/Y7uCanMT9FEHi4nR4TFM/2b77zCSUUEREREZHDNLh5/eMTlvVsLQcHQ31mFs9K2bz5JoNBHxNHMcVwwNbd2/R6feoLJylPTZMN2oSb9z6ohCIiIiIicph695efY9DBL5epnTpH3Buwcfsmo1GboigwtuNYwViNURSxf3+LuDOA8TGqi/PsrN3+hdd+2dYgIyIiIiIih+LVXzTPjbbvPztzfI5gepZw74CdjW1GUUS1WsWxHYxbLuNfOEttfIrAqxPHFr08wj02xtzxqerq3/zo/6qUIiIiIiJyGPavv/WpRi1Y8mcm6A+H9PMCpz5OdWyS6aUlStUqJs9yqFWYWjxDc3KBTj+iHQ4YmJCJ47OMZbnz0t939QCmiIiIiIg8UK98vHk+7/dmZy6eIwkHDMIBvcLCG59k7PgiVOsUWYZJh4Ni59o79MOIUqnG2Mw8pZkp+m5B4TlMVhsfYnvvvJKKiIiIiMiD1F3fuDQ5PvZxU2TkBtzxJs1jswTNBiQZ+29ep33QwvHqNcvfbtHvvoQ5cRJ7bppSuUaUumQz0zhnpi4M7uyd/+FHZ88DN5RWRERERER+0t788MzF/b37YeUDPzc26u0dFGtrlK2EkRmysbVLbaXD5EFBUWvgFAXUTy8QHWwx2N+gt7PG2MwU5dkZktsrBJdmw5/7G+c7sMWdSxdVV0REREREfuIe/9Pta+Bfu/1YPuksb2GGEWlri7Szh0tGz7UZe3wB65rB2I5HboXkZkQ66lCKIjZfvcrO31zG2t1j/5WXv3ztS8cmlVVERERERB6k61+cm+xfvvKlvXt3uX7nDV5ffoOw02K8P2IqTXGsiDzPcPK8oHX7FmnVpladoXL8LFNJRm9/l/U7dxj0w2ePndn/FrCnrCIiIiIi8qAcbO9cbN+5/9NFf4/yQoPZR5dohBnWyiY72+tsDHt4WY6TZSlTZx+ltbVK1o+4+7d/R54VTC0tMvuud/9bZ2Hxxce/FV1WUhEREREReZDe993sxZt/7/0kK/deilpb/+Pen/0t3TylMjVF0qwzt3Cc1kt3cYxrILQppSU6uaGydJ6x+TmyakBmm2uPf9/7lnKKiIiIiMhhOPfn6YtvfXCuWi0F1Obm2Gitsbm7y4wp43Q9HNfDKfKMZGMba3aeiblZ7EaFfKxCv7vPxJkzL7w9m5/fXOsulsZO3kPbyURERERE5AG48aGTT7Z37593Hpv7gVu2X9x/vUWpXMWfXuLY0mnMK6tkywckUYSxAPfEAsHZRaxKQG/Q52BtHTeMaV29envn+tvXrXTU+Onn72mAERERERGRB+L891au9v0RG69e2R29+lav3g7xtg5gs4VnOfiPnyF/8iQFYIxlw/wk7bxHf9Cis7WFP4qpFVCzLJK99e/UJ/2OsoqIiIiIyINk10xaah18pdbtU/JtrDwl29yh2NglyYZYZybxAh9jFQVJv8ed5XdoHWwz3SxTsnJsoPXG2ww7vZl3/4n1PSUVEREREZEH6We/732L3YOTu3duU3gpoRnRmG3QPthl6523CXd3wXIw4aBHdvM+zQH4WYQ97uBWobu+Rm8vWV5878f+mXKKiIiIiMhhmHzfz/8/+3v9G+HyBnUPrEZB7vZoRH1Kr7zDsD3CkBeMyCg1a8wcO45rfEbbLbbXNnBPzF158i9GP1JKERERERE5DE/9VfRidX7+yubaOv39LiW3xtz8Iv7kJKPAxtgGgzGUJyeZPnMGx6swuLXG/nYLq94kePT888ooIiIiIiKHKXj07PNWrU5nbZ/ujTU8AppnThFNVzHGwpTqddyJcXILemvbZIOMsdmTdB2P+sl5PXIpIiIiIiKHylk8fnngl14Ym1uiGBR0V3eIbENwch5j2zh5UWAmxhmmEbbtUzpzksyxqDXq/8oaH18GLSYTEREREZHD89g3Wst/fu74j4Kg/pw9Nk0SdmkPhrhT4zilEsb2PFIL4iTDn1ug8ANa/eG/PPbYE7/72L+7myqhiIiIiIgctvmnL/32IEr+GUEVag3wShSOi2U7mHQ4xGxuUi7VyFxDO42/WDp37vnHv3mwrHQiIiIiInIUHvnG3gqLZ16I4NNFqYznl+nf3yDqdTG5bWOCOp7t0Y6jf1ocn7725PP9q8omIiIiIiJH6anvda8Op8aXu3n+eSuGWb9JkRcY23HADSjygtLs1LX3/HGky/wiIiIiIvJQuPSnoyv+1NS1WrmOm9u4joPJrJTW2jqrey2M7/eVSUREREREHiaWC2sbG2xv7IBtMEMrp6iWqZ65QJbkzts/Z39QmURERERE5GHw8n/Fh9J4MFs6fZqiXCXOU0yR59QW5mn4Hlaa/Z0N/PWnGueVS0REREREjtLf/lL1SWPcfsUvfbeeZTSPHcNyXIyxbSzfJY5GuGNN4tbBXxKOJpVMRERERESOkp3mTtbu/hBjU3gOuQ22ZWEsY7CsgtSx8Pp92msblC07VDIRERERETlKUZ47e3fvQZoRpRmWBbbrYuLhEDtPCeyCaL8FwxA3yR0lExERERGRo+RmBUWnT7a3j+06OJ5DnmeYoFTK8zRme22FIo6pYrBzBRMRERERkaNlFeAam3wU0r67jMliijzHxGFk9ne2ibOYXq+DFYY4vqdVyyIiIiIicrRDTJY7Hoa8PyRLIu6trmBhYRzXpTEzSakWUK6WSSnIe90FJRMRERERkaNkW3Ya7h9QrtWwSmUmFhbAGIyxHaiWGJ+bphT4WAWYUqmtZCIiIiIicpRMnjuT1RrGGObn5oAcLHBs1yXtj4iDKvbELJ2JLUxoVExERERERI5UZOVO1BijOnecrMigSDFZjmMbg2s7pJkh93z8C+cIc0fbyURERERE5EjZnt/25+cxxiG3XSqlgkFuYbIoxoQhFpCEQ4JK6d1erbSmZCIiIiIicpQcv7ZNtf5+LIiTEaP2HlmWYtI4ItzewUozwjR7f0juXPrO9oqSiYiIiIjIUXr6D7a2nFJ1qz/ovz+Ph/T3N0nTFGNsm2Ga0dvdpVzy+89+P7ysXCIiIiIi8jB493dby16tujXYWcch+fE7MUmSML24yFi9TtLtaLWyiIiIiIg8VIr9vQtTYw2m52dxfPfH78QwGhIN+9jDwcxrP598XJlERERERORh8PrPm+fsaDiRDkOyYUSe5pisKIh7PZwiJQ/7Xy067cXXf9H+oHKJiIiIiMhRevVD5rmis3s+GfZ/p8ggHsZYxmCqE5M4M1MUSUjFtank+W8Ob9/5+Tf+69pFZRMRERERkaPw9j+sPDm8f/3jNdJ/V/Zd0jjGaU7i12uYqN/HJDFpPKK7vYXretSD0q+vv/nmZ5RORERERESOwtobV75U8/g1Y+fsr2+QxQU2DslwhPEHCe0wZHBsnDxPGL79Bu79W8Rvv/KlG/9N47zyiYiIiIjIYVr+1MTJ0g/f+pR/c42dO+/QKXWwZi3C/jpFNsSYOCa8vkJj6DA5PoPl2YTpAH+sNLl9//aHlFBERERERA7TwfLNj1ZmmpOWU2BZOQvHFij1E0Zv3SGORpgkigpndQ/r6irFKKV0+jTJRIOelbO/uvqsEoqIiIiIyGHabW0/uVz06E2VmXrkEfydEd6dA+yNPmmcYnILq1lpEu516G/vEh/sU5uZ5cTEHGy2lt74ZPVJZRQRERERkcNw9WP1i731jUsTiyeoTIyRb+2Trx4w2mjhj43heC7GKZVx3v048UKTTmAIjUWx1WaqfozjI/vSwUuvf04pRURERETkMPRee+sz05Z/aXF6nrQ9JO7FRMYhnmng/NQjeIGPMcZQeDDx6CnKJybZCru0Rl3SgwPGL1yA4aj56i/XtW5ZREREREQeqFc/Nb5U9Lqz80tLDPtd9kZ9DuIh9rFxyudPElYdjOPiZMMh6coK2ZkFxsbqWE4OYcJgmBHUXAhLXzi4ff8yNK8pq4iIiIiIPCijG8vPeY3KF/Kqz/5wH+/YJA0TEFQmiTo9RjtbxL0exi2XGezvsfvGm0T31imFKbV6FXdmnOzEFAsfeN+CsezwlQ839DdGREREREQeiCufGFuysJh6/3sWmZthemyK8doYeZ7SXr/H3u3bsLaL53k4cZzQPDZD3t2j+/ZtSp6POzeDf3yOnZ1t7Asz2c+9VvsadLjzXsUVEREREZGfvEvfPliG5vL195WqyZ2bVIYZRWefndY6/ShkutRgojFJy7YxtuOADYULtdzCX23R/uGbbPz1G6Sbbe7evfPzV7582lFWERERERF5kF7+Py8Gy++8+ZnhRovBD9+i+/2XCLYGBDlkRUxRcsjzAieLIopr18nTPhvvukD1Z5+gNipTenuL9PIy4f3WZ/La7FVAd2JEREREROTBubdzsflXNz5pHeySnp7C+7lH8aKQYGeLnbUbhJ01nCTCMbaNtTiP198nWO2Q3h6wuTdgYnGB6ANL1M+d/sp7/qirAUZERERERB6o9/zR8MrlDz/x29nqyodGmzsc/NlfM1Ot4wUu1dIY9WMT9F7exfE8l8j3GYxcvIOMenMG56cfI1wco++FTD269H2+vaKiIiIiIiLywFWeOvv9KOkzVmoyM7XI5vp9docdZu0SpaFH6Lo4cX9Ab3+fYnyMsYV5KDcoxj1GxYBj84uYtf3zf/n3kpPW1LErwLqyioiIiIjIT9qVz58MdjZufiRZ210fm10kP7iHmRqnfLJJ1u+Q3V4n2eySRjHGsixKszOMXTyPc3yWVjFgu+jTGnZJr90h+eFbr9dXe4sf/M+7GmBEREREROSBuPQ7K2F5czjpvLz8d/mby0SDLgdhh71wRDA5Rf3ceYpTx0nSFGO5LpXpaUxuE97dwN1sw3aHab9ClMd0yX7DlGt7yioiIiIiIg9SOZjc63V7v5U5KfmET9Tv07y+Q+PaDlZmwakpSpUyxrIt2OiRv3SLfBjjlnwCLKqFT+fOOvub208+9Zr7dSUVEREREZEH6T2vZN9q98Pm+t27WEVOybEZd3ys1pDk6m2crQ4AZtjvwc17mM0Wnc4e1mSFWrlMenuVZHWX+Wff9/9RThEREREROQzH3/X017LtA9jYoxr42E2X0WiPrN0iefMWo8EQE7g+bZOQn55g6uJpXGNj1vbpLN/HnR570V06dkUpRURERETkMJQvLL7oNWs/OnjnNsnGNnktwL+4QHKsysCKgQKTWRbxiTEGTy6Ab2Ot7TBa2cIrl7Dfde5PLnxlU/dhRERERETkUCz9h3th+eLSC6bkE2/uMlrbwJRsSk+dpr9QJ04yTFj3ac6dopk1CNf36Q9aDOfL7JZSpmZPv6CMIiIiIiJymCpnH/96u169HNVcrG4bs3aA07OYPH4evz6Ok6cp3liD/W6HcrlCqVbHLvnQ7vxbUxtf1tMwIiIiIiJymB79eufGX5078aOJwHvGFBZxHJIVKV6tgbFtTHVikihLKJd8KAXY45MchPyzyoUnv3bmq+ttJRQRERERkcM2dvFdv72b+/+iqI5BrUGcJFhOgfF9TBJFpK09CnLscoVWP/4n1vSJHz3x3ZEu9IuIiIiIyJF4/A971/yF0y/sR/kXc7+CX64Q7m6TxzEm6vepVMpYrkNcWJ+3GxM33vWD6LKyiYiIiIjIUXrXnw6vuBMz18I4+2IOVCo10ijCOK5LWlhkloNdbaw//ZfZi8olIiIiIiIPg6f/LLzsjU0s20GFOEmxbRtjGVN09trs9yJMrbamTCIiIiIi8jAJqrW1ne0WB50BAKbIUovCEMweZzCKmq//0sRJZRIRERERkYfByx+rPjmKwmblxGniEIq8wBhjUT6+SBmbUrX+d1k0mrz8y43zyiUiIiIiIkfpymcnT9o2pMZ5xS1sJs5eBMvCYBvsUhnjB9hpynBr7xXC0aSSiYiIiIjIUUr6g8nh3v7rge1guR5gsGwbE6cJVhJCOMBJEsztdYphVFUyERERERE5SvGwcIZra3h5ilPkGAuMsTCWZXCNRRDYRN0W+bCHsU2qZCIiIiIicpSMDYyGJHvbeL6Nm8dQFBhj22ByNu/cpBi0KXsWtcDrK5mIiIiIiBwl31iplUXYyZDW3dvYVoplWTg5hvbqCnm1THcYwqhHyeSOkomIiIiIyJFyLbIkpN9r0y9y8rCPBRjHc/Enx8GxcDwHv1qmyNNAxURERERE5CgZcscv+ZQqJbzApTRWJy/AoSggCDg2eQx2Dtge3sctFExERERERI7WcBQ2R0kM1Qq18hQZ1o8v91tAgWFUWGRj4zA2QZToOJmIiIiIiBwtq7BSp9rAqjYgqEKSY1wXk6cJxriECTh+hbHpOXJsbScTEREREZEjVXa9fn1qhqJSZ5RmVKpNiizDZHFMMgoZ2S7D/ohytfkxx/fbSiYiIiIiIkfJcYN2UG9+IhtEuOU6+90ho+EQ44QZZnmN6TAndJx3t+pu/9J3RleUTEREREREjtIT3+7cGFT9QduKP+z3DrBX7mIlGabilihFGfn9NeqOnb77++mLyiUiIiIiIg+DZ/84f6HZKO8N1+4yNuxRLZUxduBjTi1gN2tE3f6sMomIiIiIyMMk3u/PVhpjmJMnyI3BRFEEVkZiZTijUePlD+YfVSYREREREXkYvPGc9ZxpdeddzyeKR8SjESanYDjoElsZbp59w93dX3r7F4NnlEtERERERI7SWx+rXbR2DpbKhfkPURQT2WA7NiYYa2ImJxj1O2RJRM21f3N06+ZH3v7c5EllExERERGRo/DmfzvfHC7f+mg5z/6DZWyG3T52o4FfqWKG3R6BZXDTlNH2JpZv41vZr29eff3zSiciIiIiIkdh45XLv1Yq0v+XVXLpb6yTDfvUCkOeJBgnTuCgR8P1MRTcfu0yJENa77zz8Xd+dWxJ+URERERE5DBd+9zs7PDu7V9Ie/vcfeNliqTPWKMOnR4UOSbpdBjeuIWfOzQmmjTqPr32NrOec6l3c/lDSigiIiIiIoepuHP3Z6aK4tmku0ulYmgcn8LLc3av36bb6WI8282TjV16d1YgjZg4d4pKvUIxHGAtr+qCv4iIiIiIHKrw5u0PucMhQa3M9PlFsqhPa2ON4foGFAUmNZhSrUKyu8Pg/g5ZOyKYmsHMT3Gnv/rs258e15EyERERERE5FLd+deLk5v7qs3mjRHVmgWhoOFhr09vr4deqWMZgnHIF79JFoskycTkgSnLyUcrc7DxjjerSnVcvf1kpRURERETkMNx4+fKv5WX3/PSj54njmLg9IDcupVqF2aceo1qvY6ysIK3Y1J55jH7NpTvsM9rvkBz0OTE5RylJg1c/MzurnCIiIiIi8iC9/MtT80kcNk+dWSJqH3DQ2iUkwa+VGDtzEioeeZpiiixlsLVFalvMnT5FbW4Wp14hzxIcz8GzzZdX33rrU0oqIiIiIiIP0t3btz9Urpa+UNgQFjlBpcr45ASNUycwvmF/c40wDHEsx6ZY3Wc4iMnHxgmmJvCOT5KlIcxMM5VX3l/cay1d/sWZi8A1pRURERERkZ+0v/r08Wbnzqi/eO6Jp0x/73XcFrbjkXSHtDc36O1sUUlTgiDAKSxoTh3D3t+nt7fKweoas2dP4E/V6a3ep/7YT639zB8kP4Jt7rznMdUVEREREZGfuJ/5xmobSt+8c7E6my9fx3guyeYOu6ubDEZ96hMNJicm2C8KHNtxwLMhjagWhqTV5+5fXqY6M07z+Bw9++ZHYfy3lFVERERERB604fVbH2Gvxf7dOwxaHdygTK1ewY1iLGOR5zlOXliE95c56OwzO32cEycvANDd3qS9tkMrsj74g19ZfPHnf6+ro2QiIiIiIvLAvPJfTc3f+tvXnqu1Npio1ZhbPI2xPUa726yv3sENhxgLjG05OJN1KpNVYitm++473L12hcxOaSwc4+xjj31DA4yIiIiIiDxo7/6j3fUzTz72jYkTx0jSiLU332T/7m2SJKExMYY/PUlegJOlKY5lKOUZ3XxA2KzQeOwUpckx7FIVb3Hxr2BTRUVERERE5IGrnDr5N/lgF1OvUZrt0l/fYBT28EoOjm2wLAsniweEuynd8bN4xydpTFZIrJwsLQhmz5ANU/+Vny0+xNTUdWBFWUVERERE5Cft5scml4bdjSd7RXzFe+JxktdvUJ2fwj92nFanRbfbI91sY/Ick2cZ3twcY0unqTUbDAdDirQgGUTEK+tsvfTy2vb2xqWgGnSUVkREREREHgRnorre2ts/s3n59XudG/eIipzeoIuVJEyPTXJi/jjVE/PEWYaxSyXM9Bh+xSU92CPZ3yda36Rhudj9Iall/rU3Nnn9sf+01lZaERERERF5EE7/zr3QGh+7V+1k/7rWzTBRyMHwgPbOJsXuLjZQmWpQqZQwVl5QpCEr16+SjYY0cpj0y5goI+z02d1vnfn7f+N/S1lFRERERORB+rkfBt+Mdvtn8pVdqqUqbtkFtyBOY/beeZMiGmJsB1PESdFavkG3s0uvtYsplTFuwKDd4d7qKqeeed//WzlFREREROQwHH/y6a8Nt/deiFZ3ma2O4dXKbO2t0R/2ad24Sa/dwdiua/U6B5THxphaOIEfVMj2D2itb1GqNb/WmJu7qpQiIiIiInIYzv1N/D2rVt3Ob90nvreJW68wfvEM3kQTdntYBZh4OGRieob502dwvDLx2hbdgx5WtYY5NnPt3G/fSZVSREREREQOS/XCmW9bnsdot8X+5i52pcrUqSVqE8dI0xRjAp/q/Alst0SvM6QzjEj8gDb58xNnl76nhCIiIiIicpispeOXW4EhrVbo7/XYW9kGp4R39hxBuYSDZZFWG4wSC9tyaS4uYbsuB72DG0/94UBHyURERERE5FDZE/X1dHHyt+qNxpfro5QkiolG4Iw3sH0f49aqPz46Zjl4tQZ2c5xOmv6L2YuPfEf5RERERETksJ3793fS4PFHvrOTZv/KK41RtmsU2MRjZZxaFdPOhzgb9wkGI9Kqzx0TfnFrfura099JX1Q+ERERERE5Ch/4A77nNs+/0DXBJ+LxCjgj0rVbjHodjHEc8FxMrcooifFLlb2Pfs9+XtlEREREROQove8vhj/C8wZJWnze9ct4pSpFUWBs1wU/IDZQHhv78PTc8b9RLhEREREReRg89dfFC3alsm/ZHmkGxliYZDRi0O2wfXBAXq2tPfI7m3tKJSIiIiIiDwtnZub1bndI/6D34z8xxtiEeUb1+AKjbm/h9f96bEmZRERERETkYfDqr0zNj3q9BW98HJNBkRUY23GKysIxPMfGeP6fOmHUvParC03lEhERERGRo3T5cyeqaRJXHb/0dyXHJVhYoLAsTJ7nluU65J5DyXbo7rVf6W7vnlcyERERERE5SnFr7/xgZ+d6kacYz6PsOrieiynyHNcxFFGEmyT0Wi28HEfJRERERETkKBV5Qdbu4CcRsZWT2IYiyzBeuVxEoyGubRj2eoS7u/i+31cyERERERE5Sh4Ww80twtGIJImxyLEc98fHyQLHJe92cPda1C2LLBw0lUxERERERI5SnqWBk8VY+/vku7u4tgOWhWNlOd21bYa2T9OU8MlwSl4fRqomIiIiIiJHxvdNWvegZGX09rpsd4YwDHEKoFKvMUwLLL9EbmyKNAuUTEREREREjlRh0gRIXJeiWqbeHGdgDMZYhqJeY+b4AnGW0isS4izVECMiIiIiIkcqCZMgSXJyLMZPnaAoBVjGYAogD0Oycgl7ZoasXich13YyERERERE5UpnthFathjU5ycjzyC0LjMEUaYLr+Qz6ffxqhfLCPJHjajuZiIiIiIgcKTsvnPrxBdxGgziO8f0AUxSYdDiC0ZDCdkmTmEaz+VPlsbF7SiYiIiIiIkcpGG+ueBPNnx2GA1zHIev3yeIIk+cZ8fomFcelnyTvT1w7fPc3d9aVTEREREREjtJT/8fW1sgqnNzm/YFlMVhdI01SjDGG2NjsLN+l7DjhT317cFW5RERERETkYfDTf5y/4JM7ndu38MIYY1k4GEN9fILK7Bx9rVYWEREREZGHTFok1fF6DY8SkTEYNyvI4pw4yolX1y+99RH7g8okIiIiIiIPg5c/PPpUvr573s5cosJg2TbGGiXglhn1Q6bgN83NNz7zxsfDTymXiIiIiIgcpVc/Y54rrd5/tjZM/o0VWfTdgCRJMI0Tx8kDByvqgFfg1spfGq3tXLz2+dlZZRMRERERkaPw1hePVztv3fyIcexfsxolOt1dKr5LudnA9FotXAqifpv+oIXtGSqD8Nd7L139jNKJiIiIiMhR6PzdlS9NJ+b/Erg2+zvrFEQEJiONYgxJCt0uzYkKnajDxvp9TH9E5823P3nr08cmlU9ERERERA7T25+bb/Zev/oZd6/D/soKadyjPlaCzgFWUWCyKGJ05xaeWzA5M0k0ihiub1MtsmdHq3d+XglFREREROQwhfeXf6FuuBSur5P0R8wsLpIXCd3Vewy6XQxZTm9jnf3798njghNnHsWt1nCKhMHdmx9VQhEREREROUyjlVsfctMh/liD6UefZNQN2dncorW1gWVZGMu2cetVWjsHHKzsUfQTKovHYbLK6u03PnPtl7xnlVFERERERA7Dm7/kP7O1/M4n7bEAd36G5KDLYKdDd7+HKQe4nodxazUaZ89SqY4RpD6jg4iCgqlHFmlONp2dV1/9glKKiIiIiMhh2H71tS+MV/xm85FTxIFFbnmkvYySX2f2sYs4roPB8zDTU8yePEfNG8PJPdZa+/TiHgtnTuD1ejPXfnlqXjlFRERERORBevVTc5Pl/mBy4fQpRmmX7c4eYZgyPn6chVOP4o01Ma6LKUZD0vtbJG6BOd9gcMplUMtpjWIsfwrHn/ro6pU3Pq+kIiIiIiLyIK2+evVzxm18MrXHOEgcwokqgzkbzlQp8gHZvR2SeIhDFDNa36LXb9GcHcebnuDE3DzFKKWYP/3u6rydtte2nrz8rul5YF1pRURERETkJ+3KP5hc6t5thfXzj2HVPCqby3gk+MYhu3+Xwf0W5WGGIcUYz6M2PcVYXDB46R34qzeprHUpWx7tvfVnLn67f/VDV6pfe+YPdjTAiIiIiIjIA3Hpj/eWpz9w6SuPPh9bm+sruHHB5MYQ9y/eZPS3b1PPMsoLUxRYOHmeg+9hRRaMl9ge7rN6ZZfq2CzT04vnb7y39vHzL7nfUVYREREREXmQLv77u+m1d4efKr1xQP/+HXqtTcIyjKbK9McK5qc9jLFw0iiiWFvlIGqTTVSYOXMOD4/0/gHp6vav9fajxdc+9sFrT3/3YFlZRURERETkQXn9I83zrb/84SdK+9uUJjzcS09hSjDc3GR4d510LybPCxyMwZoax98eYfZjvN423Sgmrtexz839ln3mzPc0wIiIiIiIyIP21J+0b7z0i4//9uiG3Y5W17/sXL1JzfepphFutYYzPQZFgVOu1YrEdq28XMEaxCReianjJ0jGm3Rq5eVn/kxHyURERERE5HC8978UL3z/A2OXatMVyrstos19ir6hZrnYdg2MhXXtv/pQsfD4Y/SnmlTGa5jxGuVyhWyU4p19fAHbH9zf2nzSnp2+Nr/46J6yioiIiIjIT1r/xo0m+9sXqzPjy30v8/eX37hXrTjkgxC2OpjVNn4vZfDS3+CkaUK5MYl/8SJ9M2LgpXRHIY3Ywrl+by3eG+AG3n9fm1m4rLQiIiIiIvIgPPGNtfafP7F3yXrn9g8r09MEiaEoDNu2g31ynvLJM8Rv34eXwDhBgHPiFH5WpnR/yMTNHsH6gMyCXXvILb//G/lMffmJ/20lVFoREREREXlQ0mZ1626Tf932BozyiP7yNpP3RtTuDGkMPcZnj2OMwRRY0BsQX3+D1O3Qb/bIgn0aXkJtY4vq3mDyA39lnldSERERERF5kD78V8E3/a3WGbNzn8o4ZA1IrBwvHNB/5+8gW8V2yphkMCBdW6F9sMfe+iqeDWMzU0S72/S2t5m7ePFbyikiIiIiIofh+GNPfL21uUlvf5/G5ASOgcH+NsNRj907txj0hxiTF+xtr2NPNJg/fY4xf4xoZZ97t1coGvXvuEsnX1RKERERERE5DMHpxRf9scnvbL9zj+jeNqWJcZpnTmFqVQ72uhRFjsnynNLxGarHZ/Fsn8HyDlYnp9qcoViYv3z2f13tK6WIiIiIiByG0/9pte/PzF+Zbc5jH4S07t4jb5SpnzzB+PgsYGHsSoXKuVPkVY/BfgerNcJKDL04p7609IIyioiIiIjIYaqcOv/tQTt6MU1yOqMhq1trpI7D5IlzlEtljOXaWPU67SgkKyyCM+cpnzyLNzHzFX9m9qoSioiIiIjIYXr0DwdXrfGpa9WzZzh2/gyW7zFIcyiVMbaNsf2AzHGoNZqUJ8YwtRq9NPkX4xcf//q537qntcoiIiIiInLoxp566msjK/sXduAwOT6Obftk9SYm8DFJFGJ12oS9HtQqdIvon6ZTY7ff9f1MF/pFRERERORIPPKD6PJgqrbWiwb4hcFODclBlzyOMW6phJtb1Ct1unnMoB60L/3Q11plERERERE5UpXHz/1J0Kj90yJMqARVinKZLEkwaZZQJBlZBt7E+McmL577E+USEREREZGj9thvrrdLiyd/5Nabn81yiPICJwgwg1GPbgpre0Pc8vjt8//zWlu5RERERETkYXD6q/2r0dzclb1Om2C/z2A0wGAsBqMh9ePH6ezsn3npv/aeVSoREREREXkY/OjTwTO9wWC2OjVONOoBYCzHpnriBFXXYWJy6rtFkgR/8Y8a55VLRERERESO0iufmZllOGqWguAvvWqFYOkkxrExruPh2Ra5a2OGfZJO78/zYdhUMhEREREROUp5bziRH3T/NE8SQsfgeDaO4/74OJkXuBD2sYqCuN2DgYYYERERERE5WlEcNbNWh0peYBUZiWNhWWCKvCBJE2xjKKKI3p0Vmn55T8lEREREROQoecZKu5sbpP0eZduQFwkAjmUZbAPDXptip0MtCMh0nExERERERI6YbeVO2XXID1oMo5j6+CRDy+AYx6a7u0PbOMwYF68A31hAoWoiIiIiInJkjGtS24IkjhkwpNdbIchzjGU7BJ5LyTaUPA83K3CNEyqZiIiIiIgcpbxIqq7nUAoCnOGIiVIFY1kYY9sUzQb1pVOEwxHJMCYdxlUlExERERGRI2Vb6cHBAXGcMrl0Dr/WwAJMFI0oLEhcF3dxia5bJ89LqYqJiIiIiMhRGkYQlsawjy8R2gFhYVFYFgYKAssh7IzImw3GL56DwG4rmYiIiIiIHCXPDtrzZ85hV2qQx1RMBnmOAYpilFDxSvT7Hayp6ruHFaevZCIiIiIicpRsv75eak6+O0kTICFNQ9I0xURRZIX3N3CjDKvsP9b3Cuen/6h9Q8lEREREROQovef3N/cKr7I3HI1+quQ5HKzdI0kiTKVaxXdcdm/cxB4NJ5/7o/CycomIiIiIyMPgXd/eXal6brh5/RquU+A6Hk6S53hjdY7XJ4ldJwTd6RcRERERkYeHb6x0bnoSJ4vZNBYmHY0g7JOEHdL9/aWXP2J/UJlERERERORh8PYvli8NN9afSZOEfDgkzzIMBYRWyigZ4Gfp7wZbraU3P1y+pFwiIiIiInKUrv2DypPx3uaTHtlXizgjScEvlTBBrQ6NKpYHdj+kNMq/mt5Zf/adf3zaUTYRERERETkK7/yfFprdzbvPOcnwq44FZAVedQwcB5N0eniOS2fQp5sNMU5CuYh+s/vqK19SOhEREREROQrdly9/uRIn/8YvCvr9Nr2oT2EDSYqpZQarFeO7JfbzAzZ238F0VrGvv/mp279cv6h8IiIiIiJymG7+d43zvP32J8v3Njm4u0Iv6mOXDcR9nDTHRMMBxb01JoMGC41phsOQfrtFHocfTHb3l5RQREREREQOU+f23ecKikvDzj5FkTHVnKRpV+gvr9Pr9TCGokjub3Pw5m3y3GLxqccoaiX68YCd6+/8khKKiIiIiMhh2r158yNZNCStBUxeOIuVZgzXd0hur0GeY7I8t/xSQNLusr+6ynDYp7YwQ2V8jPat5edufmripDKKiIiIiMhhePsztYvJ3dVnPN+nMT9NnEXc31ynu7NPrVLDWAbjlcpwahFqZeI8oxcOGZicydOLjI+Nz2+/9NoXlFJERERERA7DwUuvf67u+JMTF86ReDb7wy44NoVr4y4uYPsexnY9aJZpPnIa33cpspyt3R1CYzG1dBprY+extz+pvzEiIiIiIvJgvfWr0/Oj+9tPzp+9QGLB+v4eaZJRdj0mlk5izYzh2DYmCyPS4QGFkzK7sEil8CmVqvSHQ5yST/PY/Cf33rr5CSUVEREREZEHaf/arQ/NLCx8yPgew9GAoFym4ZSZnlnA9mwGgz2yLMOhKNheWyXu7LJQn2VsYYFy1id0HIrZxXc7e3E12tp58q9/ZWoeWFdaERERERH5SfvRZ05UB2+84TjH5z9mTZe+m6/cYKrUxOkZ4o0Wuzu3CU1M3bJwjOcyPjPLbueA8K2bxPc2sX/qcSLf0G7tnv+5Fytfh/EXubHLnV9XXBERERER+cl79uv3+9D8yvJ/txhsvPMSQbPOqJNQenudwf37VI6VGV+cZWjfxsFyCMoW5WGEVSTkO7ts/+AvKE7OUK9OPnfjmbPh+cv+t5RVREREREQetOTm7V/wNrfZOtghX9tn4SCnVAoISymmkpOmCU6WpIzWVhmFB5RqTaon5qjWS4StXfa2bn1hZ68389pHn1t5+vnhFSUVEREREZEH5aVPji/d+usffNrtbDJjbCbOP4rVj4m2N2j39gm3ExxjMFg5QXkKsoDtPGErGXB/+S7t7pDKyXlq73/i6xpgRERERETkQXvvt1rLjUvnnjfzUxQZ7Ly9zEFrj5YpsPOAsbROUYDjBB7G8mjUpli3UiLXZvrUaerNKnnVYB5b+gHf21JRERERERF54GaeeuybB2HndyeDcYZbHdqjPsO0oOz4+O4EhWVw4lHIaOU+g+kxxo+fxq2UGA/qJMM+/qnjDNPc/sE/LF2quFM3gL6yioiIiIjIT9pbv7pUHYy2l9LBaH1i8TTF4B6VpSZWNKScJ/TvrdJbuY+VZzhkGfFkg9qFM1TrU8RJRCcaklsJpb0D2ldvrVEa+yfeI7PXlFZERERERB6Ex/7zcv/Pnuw9Gd14+/WxyRqJk2FlGZVmg7LtEjguo9UVkizFuH6J+hMXCGoB+fYu0comnYM9LCuH3RZj/eI3/NhJn/7du6HSioiIiIjIg1L3qv1SO/0NtxuSpSGbozZbu1v0W228SsDYxTMElQrG2IZk0Gdv/R6DcAhFQd12KAdlonaPrd29C8++Vf1tJRURERERkQfpmZetb0Xd/kK8d4DnlnALsPKUfNhnsLVG1OtiGxuThSGD23cYbG/S3riL5xsa5Qbp1j4ry8s03//uryiniIiIiIgchrH3PPXba/dXcFsDTlUmqRqbrY27tO7fI7qxTL/TwaRZjre5w5hlmDy7gDc3Rrq5TXjtNs2x5ovl87OvK6WIiIiIiBwG+8KxK8FE8/n+a9dJVjapNWucOHeCqVIJ//YaWZZh4tGQwdwU1qMXKFXHSa/fpWi1GZQC7FNLP3jk3+2sK6WIiIiIiByGC//LWruysPBSUfNJeh2Gd+4TBE3cMxfZP3uWPM8w5bEm1TNLFPUGnZV1iu4QfJ+hY1M7c+Z7yigiIiIiIoeptHTme127AMciHg5pr25AqcLYxYuUymUcq1TClKoM99rU/BLuo8cJ8wJaO18rz81dgbuqKCIiIiIihyaYO3bFTEx+05+Z/JSLIR6FtNsdxhrjeEEJ4/gexvGZHp+jPnuM3HEYJNG/nnzi8a+f+3d3UyUUEREREZHDdO5/uZuOv+vSV7az7P9hVesEE5NMTs5gGw9cF5P0+yStNmF3CEGZrmX90+zY/JX3/BdLR8lERERERORIPP3d4gV7bv7KXpb808R2GQ5DRrv7FGmKicOQiu+B4xN7PmEpaL/vB853lE1ERERERI7Ss9/zvpUFlb3MdXFqVdxK5cdDjBsEBZZNrVFnlCafmDj/6LeUS0REREREHgYzp8/+iR2UftYJfCzXgqLAUBRWOBqwu7NFZWpi+cJ/vBcqlYiIiIiIPAwu/MfVvilX9g52t+mHA5IkweRJUhTDAV69zEGnvfjXv1S7qFQiIiIiIvIw+NtfbpyPo1HTr5dh1MWywBSWZdmTk/j1GuVm/buuY6d/9ZmFpnKJiIiIiMhReu1zi4Ebp4EpBT906hX86Qls18MwOYY9Nc/IauCnLmP3dq9bBxuXlExERERERI5Strf5pLu/83oQFkT2BFllgcKxMFlRkBc5NjnkGa3tbYIid5RMRERERESOUpTE1V77ALtIyUdDPNsGy8JQFKR5gmtn5GGPvRtvk1saYkRERERE5GjlRRpsrdwhigb4gY2VRxR5jmO7Lo6BUbdF0mozNt6g5FqpkomIiIiIyFGyXTutBS7mYI9oOKTUHMfYNk48HBHubDGs+hRZgrHAs00KhaqJiIiIiMiRKflOOLItwigkihP2h0OKLMdYjkNQq5MnCVXfp+q6RKPRhJKJiIiIiMhRyrPcsV0XL/DxLYtqqYRlLIwxBst2qS0sEoYJ3U4Xyws6SiYiIiIiIkcpK6y02+kTxyn+1AxuqUaRF5iCHByPuLBpLp4mNC5hrIv9IiIiIiJytJIUctultHCSLKhiG/f//ycmyQosfKzc5sRjT1LkCiYiIiIiIkfLBY4/8hiF8YgTCFPIiwJTRDGO51EAuXGwSuWPUSrvKZmIiIiIiBwlp1TZM7X6h0eDkEpQpvADiqLABAcDsptvUXNHJF7x1M5kdeuZb4eXlUxERERERI7Su/6we61brm75Y80zDiHJO69S9IaYLM+ICli5eZtsFDaf/ePhFeUSEREREZGHwQf/qH81i+Pq6vUbZMbG9TwcfJdGcxxvYgKnXN6DrkqJiIiIiMhDwzImXTi+gNPu0LZtTGZbkIMVZ/RXV5959ZO1i8okIiIiIiIPg1c+6j+T7e+d7+zskaYpaZpicmNBFBO1Dijb9leH25uXXv5U47xyiYiIiIjIUXrzlxvn6bRP2kX2B5BRJDEFYEq1OkWlTKNaIhn2qBbZ73TuLX/onS+dqCqbiIiIiIgchdv/+ES1s3zzo146+kba71IreXiNOqWxMUx00MEq+QzDPsWwh5dF1MLhb+5evfIFpRMRERERkaPQevPKF2p59G/8eISdhKT9LqbIyaII4xRAu4VxIR716azcptTrMLxx8yM3/9H0vPKJiIiIiMhheudzM7O7y9c/Hrd2GGyvEbW3cV0LRn2sNMVYeUHU3sdpVhibGsfqdTlYuUs1TT40XL3/rBKKiIiIiMhhGmytv9fNkuei3S0GB/vUJscIamW67RbtdhuTDobF/r17HNy/h4lTpi88Tq3aINzbY+fWzY8qoYiIiIiIHKb2tRsfTfYPcHyfYxcu4Do+7Vu32Ll9k6IoMLkFgWURb+7Qub1G3o6oTS8wPjXL/r1bv/DWr44vKaOIiIiIiDxI3/+/PRcA3PnoxHnnndsfmmuO0zi5SJEZOjfuE2+3aFJQKZUwfq3K2MklbOPQa3iscsDAT2ieP8uiV5vd/rMf/nMlFRERERGRB+kX/u8vhABvvXH5S1vH/JPFT50lrDqEuweULBfb8Ri7cBa/VMJkSWoxP07t6QuUnBLToU+8sUdBjPXoPGtJa+mtz54OlFVERERERB6k1371eNVe2XzyydmzNHYjhiubpK5Nr+ZRfeoC5vgseZ5jiiwjPxjgJg7NEydIah7OZIPW/i71UplHjp94buP11z6npCIiIiIi8iC1b934+NLJk8/5nsted49kssJoLKB+YgE3NeTrbZIkwbFdh+6bN4m3x5i4cA5/ehxDSj4Y4c8t4PcT9ja6zcuffrwJtJVWREREREQehKTTWXBPTJI3IeuOqJerOB2DO8ho377LsHuAXRQYu1Ri7Ng8XpSwdvlVuu/cgVGCbRtGe5tYjy5NHf+ZZ/7tM9+4rwFGREREREQemLn3vecr1tNnxzpxC893qO2MqLyxRu9HrxMPBzQeXcTYNo4xBuoBRG3qgxhzY42tm/fIlyaJZioEKzc/8rPfrX1NSUVERERE5EF6/Gtr7au/EH4qu7VF1u+wfW+fZuhiFRlmYZLUzijyHJMmKazvUD7oU4wFeE+dZO59T9AY5ZTf2qL3F2985u9+uXleSUVERERE5EF6/Vem59f/6rXP7a+s08siau97BOe9pyjGSpR2O9TuH4AFjlVAUYYeKW6Wkq1s0Q1Dcsdm/NgstcWz33n699s3lFRERERERB6kp35vZ/3N9z359eHy6x/t7q4Tb75DOS9RznPuTsT4czl+AU6RZ1jVcbw052AU4nkeE5PTWMeOMfCg8czTX+Pl+yoqIiIiIiIPXOWpR77VzQ6YP3ES5+42yeouiRVju1Uq9VkK9xoOeVqE9/ctHBd7cZry/CxuUCcMU0oXzrD0P9/v/+hXypfKbnkPWFFWERERERH5SXv9c3OT/V5r6fS/X7n8+j95N4M332Hi0Ucws3MUBy1Ke3tEr29jRTFm1OlamUlwnj7DwsJxSllOJ+wwykPylU0uP7ZXpMsbl4ZpogcvRURERETkwSjsdLC6c/HPn9or8lv3CeKUeNjFlKB0YprpC4sUHsRJjAmaTSrvfoJys0xy0GJ3Y5V22MVzDd7qNqW1zlcqhdt/9psd3YsREREREZEH4qn/ba3tWW7odcJ/FWy08MKEbr/NSmuNYdzBTDU49sTjVGt1TFEUEKfs3bxNYoM/UccdjAjijLTXJ253Tr77tcrXlVVERERERB6kn3u5/PVkZ/98tt/B9XxcG+yKyyCL2Lt1GwYRxtiYPI4pbt0j7/TZXL+PSWMmqw2cXsT2zVuU33vxK8opIiIiIiKHYebixW/sra//KNnfoxz4BK7H3sYWw519otv36He7mIKCdL+DYwdMzMxQn5jCHPRpv/kO7njze/YTSz9QShEREREROQyPveR8Jy77/e31NeJul8nqGHPj00zWx4haHbI0wWAMVq1G4+RJmmPjDLcP6G3u0jPA0sLl8/9xra2UIiIiIiJyWKrnT/8gdCxaWzv0VrdpNiYIpmcwzQaO62Jcx8U+d4Jssk5nv89geQ2nUmZU93HPnfgbJRQRERERkUMdYs6eeiG0LXynTH97n9H2HtSqlC+ewfU8HLuw2G+U2Em7LHRHTJ95DNyQftL+5rGZyeuwr4oiIiIiInJo/Knpa+Va9VuT09OfzCzD8GCf1qhHpVLDcWxMPF6hWSuzULgE09NY5Qr7pvbPZpbe/ZUn/uO+HrcUEREREZFDdeH/uxKOP/Hur+wT/DOnPonbGGesUqFW9bErVUw/GZHv7BJEBaZeY2jbOOMzVz/wfPGC8omIiIiIyFF41/f5XtqYuDHKCpxyFduyiTe3yaMI4/kBxvGILIuwKEjKpU/UTp/8kbKJiIiIiMhRGj935nuFbX7J8oNPj4YhrhNQpCmmyDOccgWvUqOw7c+WFxf/6vxX74VKJiIiIiIiR+mRr95NH/9r/1uZZad+qYJTrmE5NqZIM6LegIP+gNLM7NVHfkcrlUVERERE5OFhTU7eiOKUUb9POBhgsiQhGg2gXKE/Cptv/uPjVWUSEREREZGHwfV/fKI67A0m86BEGMXYjosxtk0wPo4/3sQN/B/mg+HEq1887SiXiIiIiIgcpdf+25NBPAobfqX8l97kGH6tjG3bGK9SwSoF4BqyPKO7u3sv3N49r2QiIiIiInKUersHS4OtnbUiicmsnGCyiWUsTJakpL5DFkcUeU40ijH9qKlkIiIiIiJylOwkdxiGBLZFlsWEeQJFgcnznCxNKAz4RUFv+Q6usVMlExERERGRoxTYXrpze5k0yzFWQWYXGGNwrDSlbLlY3T75dotmPMSzcxUTEREREZEjlXidSc/qw+YGpXIdtzxO29g46WjEcHuXURAQpCm2a2MXaaBkIiIiIiJylKwiDTzXpj8aYEUFeS8kH40wtu9TqtVxsPGbTWzfJTOWiomIiIiIyJGyLa/vOC6N8TFwDbWqj3FdTJ6lGC+gPnuMXhSzN+gTqZeIiIiIiByxLIVev0+v28E7NoNbq+L4PsbYDiQp+CXGji8wtCAuMr0TIyIiIiIiRyopLKK8oHnyJFbJJ8tSktEIY3seFJBEMYXvMff4RaIi050YERERERE5Ui5WuvDUU8SBRzQaECUxThBgjLFI/DJpDr0wolKv4DQqW0omIiIiIiJHyQuCtlcJPpHFCZ5lg+uTRRFm1O8Trm9h2z5uvc7IKj7WnJm5pmQiIiIiInKUnv5W50a7SIMi8HFdj/z+OtFwiAGIhiPW7q/S39//8NTpUy8+/lv3QiUTEREREZGjNnXxwnfSOPrZe2+/jRWnuI6DqdRqTM3OsjA3S7la3XrkN+/3lUpERERERB4Gj/2be6FjWSwdP0G13sS2HYzbSxjFXaKwQ/Vu+/zyh8cvKpWIiIiIiDwMbn1s9Kny6vqlIkxpOTlpkWGyKCaPQhj1sUz6jcHm2qVrn56dVS4RERERETlKr/2j8qV4v3UGq/g3cb+PlaS4no/xqhW8UoBPyjDrYtzkd7o33/n4jS+e1lsxIiIiIiJyZMK3lp8jzf+nMBrh+DZV18N4HiaNE1xjYxUxw7hNqWJR6vf+w+DlV7+gbCIiIiIichSuPNX/XGOQBR6GzIbBoI1jDOQ5psjSgsGIJBowTPpsrN7E67fpvPXWp5Y/e2xS+URERERE5DAtf3ExGFx7+5fs/fb/2N3ZZ2d/l8KzMMMe6XCIiYdDi9YBtXqTqfEx2rsbDPY3CPLwuejunZ9XQhEREREROUzD9bsftEf9jyd7u3Q3d5iZPcZYNaC/fZ8ojjFZFHFw9x6DrRYmM1x46mmMb5NZEb21lWeVUEREREREDtP+zbc/aUxEXvY48fgTeBm0trZotTYJRyHGsgAvYHtth/7dDaxBRG3xGDQ8Nu7d/Oi1XxpfUkYRERERETkM73y6eb537/aHippFcHIK4pjO6gatjXUiK8Z2bEzQHKN0cpGKX6eae/TXt8hKhplHTpGTLvXeuv4JpRQRERERkcNw8NqbnyYcnJx5/AxRUDBs7eOEGTXPZebkDNVqBYNtCI7NM3H8OInrYbkBra0WmbFZPH+B9t17H7z+qRNN5RQRERERkQfp6j9acgb319977sITWJZNr9cnLDK8eoWJU6eozS9guy6mSBKKzU3s+QaDC9MMbYdm1qS7E2KNT1I7Nf/x/u3rH1FSERERERF5kNpvvf6Z8vzxjzreJMO1mDIVhp7F6OQU8YklDkZl4jjGKfKcdPkOw/4BjaVZgoVpCtelsMHUJknLOdFmu3ntl0/PAltKKyIiIiIiP2nXfnlhsn3jrcBfnCWcrnCwt4JrF0xFTcxehP3aVUqru+zlYIzr4k6NE0Qh7Stv0t1cYxh2SfYPoN3nvX9hWxPvfeprF39/SwOMiIiIiIg8EBd/f22v8r7HvvnuF2yr2GlT24+pbQ3Jb67Se+stsuE+9tIMtuPg5EkG42WKfoTfCtl75ybRXZ+Tkycxe4Y3/l7p4xe+tvodZRURERERkQfpqa+sta//g+CZ0es3Mb0D2ltr5FZKYhWY6TLemCHLUhxTFLRb2+wMWsyXS5xamCH3A7h3wN7eNnfj7md/77PvX/+V3x1eUVYREREREXlQXv7k2NIb3//+Px9r7TPTqDJ97hxZ0me322Kzt0NqZVhYOIVlEVTqBOGAPIe1jXUSY1OPPaZPnSBZmLv8MxpgRERERETkAXvPtw6WL186+71grfTJ3sYa3X6L1CQ45QC3MARuQOzYmMxYBF6NacpkaU5e9jDHJ/GeOk1+bpaFp5/+beUUEREREZHDMHnp8a8zN8Xko+coT4wRGB+/nzKTV6l5ExjHwfFdpwhv3rNGVQ/OnGCsUaI+PclBFJMfP8nZr663r/5C+RKV2hawrqwiIiIiIvKTdvVXjk1mo+780ldWr17/b8+ydvdtKieWcHd72Js9gvUhvL1FPBxhwuHQsn2P6lOPYZ9dxAvqxJsD3MIlGYa89NR+Mbh3/9nITQKlFRERERGRB8Gi4ODmnedee1e3cNfazCUlgv2IIChjXzjF4JkL3J10SLMUYzkO7qOP4E5MY3dCws1dokGfvDcgvrdO0R7+K+r1tfd+s7WstCIiIiIi8iA88Xube6ZZ3Ur39n7DWtvEb/fJuxm9jR5WL8KrWUz+1BKVWg3jBgG4Dp3rdwn2QipBQF6Bcr2C1R6wd3fjZ376Fe9byioiIiIiIg/Sz71U/vrB7salYdolb7oUtQp+UcXbS7Dv3qcWtinyHJMlCeH9e3R7+2zvrVPkCfXKGEVnyPb6BpPvfeq3lFNERERERA5D88lHvnX71jvk/SH1LKVaddjZ32Cn1aa9sk04GmLSMCRpt8BJKM+N4c1Nk+4dMFheo1atf7N25uTfKKWIiIiIiByG2iNnvxeM16707i6TdVrYYwHBzBi5W6a/F5KmKSZPU4wFMyePUZ+fpNfeJ93v0Ntp441PXn/0P29tKaWIiIiIiByGR/5Ta3liaemF7l6LeHuL3uY9xk/NcWzpNCWngTE2xi2VqCwt4VTLHPTabG5tkudgggr1c2e+r4wiIiIiInKYvOOnf1C4ASXPpdPeY3t7HbyAiVPn8PwAE2cZlAK63T693ohji2fxF0+TT099zZ6ZuqaEIiIiIiJymPy5Uy9YU3PPMzfP1MlFoihit90F1/vxxf7a+AT5xBi+X2V2bAHXrtIv+JfBI2efP/O19bYSioiIiIjIYbrwW3fT6mPv/fdDr/yv7KDE9NgkzWqVdKqJ36hj8jxluN8iKjzCfo5dHadfr2098zelbyqfiIiIiIgchfe+aJ5vVWtbSVD/opVakKd029sUWYYp0pTCcQjdgObkAtnB6LMzj13UACMiIiIiIkfq5/62+ZWk1Z/165NYjiEvGZLREJN6NoEpM+5WCQk/YT92+gfn/+NqX8lEREREROSoVS899TthkXy+5JZoZgEWBhMVCc4wJOz1sWabK+f+922tVBYRERERkYfCmf+0u+4tLlweDodYB0MoCoyVZoTdA9Kyy0G7c/K1L0yeVCoREREREXkYXPvS7OzO9u7FtOQTDQ5IkwTjuC7ezDh5o0RzevLbUetg8ZV/NDurXCIiIiIicpTe+sJikO7snW/Wa3/AWIN0agzLsTGF7ZBNTuD4HkUc4wxHf5kddOeVTEREREREjlK801rKB+FfuoUFrk0wPY7n+xjbGLI8xaQxZBmdVpu8P5hUMhEREREROdIhJk2C4c4eZBkmGpF5DlgWJokiHMDCwi0sNl5/A991QiUTEREREZGjFMVhc+PWTUyU4gclojQjzzKcPM8whYU1ihl0d5kslbBN7iiZiIiIiIgcJT/w+1XPo7+xQV6vYZeqZJaFY1kW8e4+mVsiHnQIymVMkTtgqZqIiIiIiBwZY+VO2fdx0ozefpfUCXEBY5dK+EEFdxgzUanjWxaFrWAiIiIiInK0Cit3bMB3fdw0Z7JcxbYsTBGnFBM25okZ2lbMcJRjIr+vZCIiIiIicqRDTGbSAQ4tY+M+sgjj/o8fu8zTlCyLSRyL5qlT9JOcKMkDJRMRERERkaMU51a6NxgxNn+MiALSBMu2MVgWeW5TRBbGqzBx5gzGd/QnRkREREREjpRxnXDx6adJHIdyYVOE6Y+/F1lKljtYTpW9Vofq9NT7k8BvK5mIiIiIiBwlL/D7tuf/vWSU4mGTuR55mmDiKKLYaZHFhv8fe38abMd5H/af39777Pvd9wvgYiNBERQhibIoi7ZoizYlkTZlUTYl0xkmkctKlf9TScVT4/8kVflPuaaSKTsTO1YixaK1RLQlRxxLsWRRFmVSIimCJEhsF8AFcPft7Fvv3fMCqZpx5Jo3YwCcmd/n7akC+n7vc/o+T/dz+uQLlameonDvX3aWJZkQQgghhBDidrr3z5srsaKGlmUfVjUTb2sX33FQ7UyaZOiyd+Uq/VbrwPv+2n9RcgkhhBBCCCHeDt7z3fD5aOgUr7x5hmgwBBR0RdfJVSqk8xXIZ3agI6WEEEIIIYQQbxspy+zPjY9hhB5N20ZNEsB3iHyH/tb6fRc+UZuUTEIIIYQQQoi3g5XHxquDvd0Tnu+QxD7AjUcsE/r4/TaWoXyutXbtgdO/UpmVXEIIIYQQQojb6a1fnq72trZO6qryJS8Y4ocOURyhqqoKmQyaHkPgoEXhFzqr6/dJMiGEEEIIIcTt1Ly++r7Icf8qCFzStolmG+h2CtV2Yry0TTNtYXR8agOP3HD32HceaHxasgkhhBBCCCFuh5ff2/x0wVs9mYtbJP0uTqwx1AuogEoYorQHpN2Ifr9NZ2uNtOP/TueNC48s/5OpouQTQgghhBBC3ErL/3RB3zx37hG13/0dt76L12qRdPoYQ58o8FGjwEfba1IyTNI5E3/Yw1vfodqPHxiub5yShEIIIYQQQohbKVjdPJnzowf6jSadZp1CpUglW0Dd2MVzXNQ4CBleu0ZvaxMlY1OZnyUdJuT6Hp2r67KIEUIIIYQQQtxS7oXLH8o5IUaSMLY4h5LScbY26a1u4Ps+ahSGDPBptvfZv3YN1TIwywVSGZvB1evvO/vJqjypTAghhBBCCHFLrPzqdNZdWb0vrWhkqmWUlM7e5nX2G9sQ+4RBiGqmM9gHZ+ilY6woJqk3YaxE6s5DKK7zwODVyw9KSiGEEEIIIcSt0Dpz7rG423kge+AAeiFHs9fE0WMcIyE/N0E2l0XVLYv85DgTczPYlok7GNDutwgJmZydZ3j+yiOXf3miKjmFEEIIIYQQN9P5T87am8vLD9WOHSYxFAbtLp1OH13TmTu8hDk+gmGaqFEUQd+lmK2Smp3DTVm4ikq708XM58lO1B7cuXrpQ5JUCCGEEEIIcTM1Ll36UGqq9ohTK9DotfDDkJJdoDY2i2llCBMIggBd0VT6r13E2SiSX5rDnJ+kYGSJ2y5KbQKt0KPt9Iqnf21uFliVtEIIIYQQQoh/aOcfn7U3z70+lpoawaxkibMxBcNCG7jgxexfvcpwv4kRRaiKqpKv1jD6AzauXGR3c5OwOyAZukTukLtftJXxu4597eSf1mUBI4QQQgghhLgpjn551R09evgbP/WDtNIjYug50Okz3Npj4/RrhN0W05MjWLaJHnsxTGoEPQd7vU92p03XuUx2fg41cVj+uQMPLX1+75uSVQghhBBCCHEz3fmVvc0rHwwfTt68QtRoMFhdJ7I0orRCnItgxCeKhujoKv7yFkbkoJRrhMUKBSuHu7ZL57WztJbX/tVLjz128V3PNFckqxBCCCGEEOJmef2R0uKPv//NT6udHcbyBYr3HscYumR3djHbHn5jFeIYVUkisDMMIpWsb2NtOuxdWmMrcnAOjmDfu/SsXSnIVjIhhBBCCCHETfWOr7dWqnNzP5gYnUYdRDQurePudNBUk36cEGfSKJqGqqgKZmWcQn6EbsvBCyJypTJTRw5TmZhg9q67vnDXH10LJakQQgghhBDiZlu8+57P5kdHqC3OUyoW8KOQvhdgGCnSY9MoqooehSH+xi5uHBONVLCPHsA2dcJOm9L0IqqW3/zRR9wTar6yAvQlqxBCCCGEEOIf2sVfPaIP9neOLfznrTOXPrbA4PpVjNoBKgE0L1wh7LRw1/ZIohg1chyiyCV3x2Em7zhK2jJxfRfN0AmHfYanXw+Gy9cesP1EygohhBBCCCFuCt8Nit7W7snX7+0mfnuAbaaJHI9A1Rg/foT8iWN0Axff91B1O4X5jiWUqRzRYIB/cQ1/bR83DBn2G2wMm9jF4vUT/3Vd7sIIIYQQQgghboo7//xKvZ9R6o1mnXS9h7HdJtlq461s4LoO+kiO0n13YNo2qqJpaJgMl1foBV0GVZOkYFDMFDA32wzOXnu5dGjxO5JVCCGEEEIIcTNNHzr0rc39reeutjdJpvK4JQOlnGHY6uKdX8XoKaiqhhr7Pv7F83Q31xmsb2OFKik7S+A5rG6tUrvj4LePfkHuwgghhBBCCCFuriNPXwunD89/r7m5RtwdkLNzmIMAmm0G+3VaFy/iew5qHIb0wwAll6WUzlDMFkn1HernzjEgXqncfeJpySmEEEIIIYS4FcaOHf+qEqsrzfOXMRtdsoUiqXQKVwFPhThSUGNNITTSlBeOkJ6dIurs0L5+GdX1mBiZf+7Yn7TkSy6FEEIIIYQQt8TRz7dWKvOHv+EqCs2tq0ROndTUGGOHj+LqOcBE1UyT6swcaTNNMPTY29gAy8a1bKrHjz8jGYUQQgghhBC30sjiwW/Fho1qZdhb32FYb2HaGWbmZtF1DTVOElQ7jdt32dmtUzx4BGt6FrdUfEYbGT0nCYUQQgghhBC3kjY2eTq0s8/YtXHKBw7T6vboDoZgWmi6gWrncySpHEQq47OLJKksXdP6PxSO3PHVg1/Y2ZGEQgghhBBCiFvpyOc32tVDx74a5cqoVobq7AKgEhVL6JkMauj7JJ0uZirL0I/wrTT6yPiZ93zP/LrkE0IIIYQQQtwOuaUjz7pW+hNJNocTg55OE9SbBK6LGgyHqKpCrGmomSzDRHk0NTH9omQTQgghhBBC3C6HPnc1zBw+8mwrCD+mWhaKrmOqKrHrohqpFKQstJSFmyQfq8zN/+CuL2y2JZsQQgghhBDidrrjC2v9/KFD33Li5JOqYaDn0himgaooKqE3oO10Kc5Ov3jki1t1ySWEEEIIIYR4Ozj2J+v9yoHF5zzfZdhukaCgRq5Hb2+XKGUx6Hbm3vrUnC2phBBCCCGEEG8HZz81k23s7R6PFQWv2SD0PNQ4Y2KOj2PWxjHt1AvRxvaJC5+ayUouIYQQQgghxO107pPz+nBv91i2Vv0rr1zBHpsBXUON0wbUaiSKihaGpNqdlwbtnROSTAghhBBCCHE7aXv1pUx/8JLiDtHzOdTSCJploidRjBKHqH4fL0nwnQHxUK+ALtWEEEIIIYQQt03XdYvxoItdMDC0hISIBFAD10ONfHQlIh35DM+/CXEgn4sRQgghhBBC3FaapbtbZ89gxD6J7xIGDiigaoaBaVvEgUvn2hVMyyRtGa4kE0IIIYQQQtxOYRxk87kszevX8f0BKS0hAfQ4inB3dnBMjVw6jaJrRFFoy3YyIYQQQgghxO1kqJprRhEZ26K/v4eXZCABFcCybOIIlBjQDVRNlzsxQgghhBBCiNsqiRMdVcOwbBJFw9JNlCS5cSdGy5dIpTMEm9t4boCpmQOIpZoQQgghhBDitonQ3EgzcYOY9NgMmqOQKKDqpkGYKGiKRXZ+kXbfY+jIB/uFEEIIIYQQt1cYxnq90aY4M4+ayuL5CSQJahwFBGiEiY7nJ0wevUNqCSGEEEIIIW47A4WDd96Fp5kEA59QM0FRUc1IRXG7WFbAIBkynKr8TJwfPSvJhBBCCCGEELeTlsq3ldLou7yBSzGtklhDFAXUxA8xWvvE8ZB8KU+Ytfrv/lp9VZIJIYQQQgghbqe7v7m37KFgWfbhYRIx2N/AdRxURVXpN9vU17YYtro/U6rWzkkuIYQQQgghxNvBvd9zXlYcr9i8uo7fHaKqKrqVz5Mdm0A1UpiW1T7y+fW+pBJCCCGEEEK8Xai65hZUjUJllG4qhRq6LgmQJCpJsz179qPpk5JJCCGEEEII8Xbw+kdzx+JOe9Y0bEjA83zUYDAAP0AlQY2irw3WN06d+0RtUnIJIYQQQgghbqe3fn0m6+/snNCS8BtqFBIMXZIkQU2VSkSmgef0IAjIqNp/GFy88Oil35yT74oRQgghhBBC3DbtN197MptEX1KigNAdoJo6ZiaNmqgqRrWEbmsE+7vYnovaaP5+/Y3Xn5BsQgghhBBCiNvh5fsGj9v97qQdBbj1PTQtQa+VSMIQ1R8MoVUnjDz8xj7x3h55w2DvzbOPXfwnU0XJJ4QQQgghhLiVLv2jeb1+7tzH0qj/3N3dxm3uE4UOtBsQx6hqktBrt0gVshSLBZTtbbyr17Ad54He+vopSSiEEEIIIYS4lZyNjVPacPhwdH0Vb32NSqlAupCn3W0xGAxQVSfAv7xCuLaGUi2hHV7EKGWx45Do0uUPSUIhhBBCCCHErRRdu/jwbMchq9oUjxxFKaZxd66hnHsL1Y9QA9/HUBRau7vsrlwhyeeIC1lUU2d/deWBS78xUZWMQgghhBBCiFth5ZMjk+trVx5sVQw6IynCjElrbZtwu40xCEiiCDVXLmGUq+i6hZFKMdzfQ8+kGDswzzBwjm2fffNxSSmEEEIIIYS4FXpvrDygtZ0TqcUZUlM13HYHPdRIEh374AKZfB41CAIyswsURieIVJ32cEi330FPWywcPsju2up9klIIIYQQQghxKwzPLT96aPogBTNDc2cft+sQo2EvzqEuzWOmUqhREIDrk52YoVQbwUhZRLrKfreFbVlkLTX83rvcxySnEEIIIYQQ4mb63nuDh5NKZkMv5xns1ckbabBSpOZmsEoVIjfEHQ7QkyBg780zxINpRiamqI2Ng6nguH20WpWRjPt4d+g+/+bHj2eBvqQVQgghhBBC/EM7+4mD9vq5V8aUpbFPB9Uc6maEUagQhRFRGKOurBNdWicJY1TNtklXSkTNBjsXL1BfXyN2A5LuECOIKR8/USsuLH77zq9ckQWMEEIIIYQQ4qY4/qXLbvX4gW9PvvsdqSAM0PoBUc9h88oVrlw6z359G7taQTMMVFSN7NQkuprgtdp0r15n5XvPk7T7eJevws7+0t1fa6xKViGEEEIIIcTN9M4vNlbD9a2T/rmrsN+j8c3voa6so3U6kFFRJjIoJOhKktBfuUQQDslli6RLFRQU/F6XzdU13Kb7L3/8S7/wm+/8c1nICCGEEEIIIW6eHzyaPXHtG9/5P440G4SmzvjSQWIV9tu7hL0WrbBPkiTooeujhiFx6GMoEPaHDIYOmqZQGZ2gV6ktywJGCCGEEEIIcbPVzNKKXRg7Yyqx3tG8B9adPUqRThWDTs8lNnRiElTV0knPHqJYnsJpdInqTRTDJHXiDrp3HmXkgx/4XyWnEEIIIYQQ4mY78pX1/r1bC/9i9d0nni7MHqXkpuj3emwEA6LKGJXaUTRNRzdUldbONu3EI1Utkpufp1Qs4vkBk5NzHPmjtf4rHxsb081MHQglrRBCCCGEEOJmeP1Xp7Pv+OJ6f64yfsZsXmXkHccphV3WLl2kHXqk2nXCIEB3Oh3sJKG2OE92bIQwCnCdAZqewai3ufguL9lYvvzJ7MLCc8CmpBVCCCGEEEL8Qzv98cni3ltnH7nwLrtt1RlEakKgDlFtncV776Wxv09rZQ01CNA10yQ1NQ8To7SaW+wP2qQTjbLqQscj8BWsjG2/6+v7m1ePS1whhBBCCCHEP7yTX9lsv/huqx9sNr+m2AqB1qEedVCShJKRpTI2ScHK0fjuW6hGJgNWlsbKFq7jo5gGmVyKdDqF1++zvXr9myPHjzwrWYUQQgghhBA3U/Hwwve6q+vPJZ0umUIOK59CURWUIKK1so5uZDBtCzVwhvhXrhFsNvH3u2QTFUMxQVfpb22QnZk8vfSlnR1JKoQQQgghhLiZjv2XrXrxwOx3ejsbJJ5L1sqQQWew3cTbabN/YYUojFB9Z0jS7qAoGrZmMZKtYgYJq2+9xVDh5eq73/lHklMIIYQQQghxK9j3nni6lzafWzl/EXPgUypUyGeyJHFC0Ozi+z6qqmoMMwblxRlGZxfQWn3aV9eIgphktHbu0Bd35S6MEEIIIYQQ4pY48JWdHXe0upx4McPLGyg9l9T0JOX5WbRMmiiKUFXDJHdokaSaI+r3aK9uYIYJuqJQXjr4bckohBBCCCGEuJVGDh/8Vt7KYPZD9i9fxfEGmKMlyjMTmIaBjqFBRqfldlAbdfIjU2i6Tr3beDY1d+A7sCEVhRBCCCGEELdMemb65Xra/qYxPvFQ1oDN3T1KRppKqYBumqhxKQdFE81MKI5OYpVmGCjFf2Hd+Z5/d/RzG21JKIQQQgghhLiVjn12q67feeTZZsZEz5WZnzuC4itExRJqLoNKAkrXQ/USItsk0hPiUvb6/X8TPi/5hBBCCCGEELfD2LG7P29kK7+BaeNEMbqp4TfrhL6P6rtDNDfCCDUiXaOT1h81l2Z/INmEEEIIIYQQt8viH14NUwuHn3WN1Ic1y0YzdbTEZ9jrocZxBEaaXLpEoKifNOcmTt/xZfleGCGEEEIIIcTtdewrW3V9YuaHbhj+hm5qGJaGbVuohmkS+gGBH5Kfnnnxzi/WVyWXEEIIIYQQ4m2xkPnqVr2wtPQtPwrwwpAkSVAVFIKdTYZRwFazceDMP5m1JZUQQgghhBDi7aK1u3fMCWOaO3vEcYyexBFGtUw8XqGWL/xVY2PnXcDLkkoIIYQQQghxu736i7ljhbR1JTAhUVQ6ioJqWCnCySqxERP1++T6fvb1XywtSi4hhBBCCCHE7fTyR1In42hY7fY61yPdIkkVUHUdNUkS4sjDM0GzUwyave9q7eGcJBNCCCGEEELcTn4wGBs09r5vWDqxFxKoBoqqoQaBT5LJoCQxZrdHdHkFjUiKCSGEEEIIIW4rTYH+pUukwxBTVVAUQFVQExLMJEHzAvrXrqMFDqoRSzEhhBBCCCHEbWUlaljwA7wLF4iHfVK6ShLF6EkU0dtv0E5gzjBoRyGJKsGEEEIIIYQQt1eM7vpBhJ1KUa/vgZ0ijkLUOEnQNQ1N04lVHSOfA9NqSzIhhBBCCCHE7aTGamim0iiGgZfERJoGgK7pOplihcRKM7i+R6BChCLFhBBCCCGEELeVomquaqUYRjH5qSlMw6Sr66iKphJGCSo6+akp+s6QzqA7KcmEEEIIIYQQt5PnhXaj1yU1M4OWzuAOhiRxgqooKmoYkwQxIQrFo0cw0lZHkgkhhBBCCCFuK0UNS0uHCHSDuD9AU00gQY3CAFQdrAwxKvlMiiRl9aWYEEIIIYQQ4nYyUnY7UykRBB52JoeRcONOTOj7+I0WqqKBojBU1Q/bpdKKJBNCCCGEEELcTnq1uOoq8aOGncKLINhvEwcBqmFb1Hd3aWxtE3nu+wuzcz+463ObbUkmhBBCCCGEuJ3u+vyqW15Y+J7nDN/fWd/ErXdAUVCNUGEsX6Lc72IU0/XDT8sCRgghhBBCCPH2cOSzm209ZbdzsUcpZ6KjoCZ+iBUn5GyN1s7GqR89WZMnkwkhhBBCCCHeFi48UZsM9/aOqUqEaoGh6aiqqpEoIZ4eU0qnPje8fP3+M/9ozpZcQgghhBBCiNvp/JOzdvPcpYdM+JJmm4RaQhgGqKZpQ8Gm43TBcRkP4i8N3zz/iCQTQgghhBBC3E79N998vBapf2zFCq1Bl6GtYWYyqEoCFLKoWR2l28FOEqz9vWOv/JT7mGQTQgghhBBC3A4v/bT/iN7uTumaRrDfQNd19EqeBAXV7/eIO12CwKe9uUa8uU3WMn+nc/XSQ5f+8XhV8gkhhBBCCCFutd3lCx+2teRfhY1d+vs7xKFH1O4QDoeoahQRNNqUswXy1Rr97XValy8Q1Pee6Gxvn5R8QgghhBBCiFvpjUdTJ2nVl1rXL9FYv0K6UqBSqpC0B6CAih8k3curuDttzGKR7IFZCtU8WhQwvHztfZJQCCGEEEIIcSu13jz3WCb0T9k5nZE7D2CVM3jb+zTOr+AMh6iRArob0l/bo3HlCmoxRZQ10YkJV7dOLv/qZFEyCiGEEEIIIW6Fq0/M2drW3jErDrDLKeKswtbqFfrrW+S7IaEfoOqZtFKaWiCdGIRRSKe9j563mD64iNtoP+gsX/uApBRCCCGEEELcCv3lKx/0d/ceqs3OohVs9to7+FpESjOojM2SL5ZQNd0gOTBFcmAStAxWQ0FdHaJrJUYOLrCytfyQpBRCCCGEEELcbGf/6by+vPbWx3J3L2GWR/FWPVK7JiknhzI9i7o0iW6BGvs+ymBIbrxK+tAkrYpJK6/jtupUyiNMeGr4w3cPH5ekQgghhBBCiJupffbsY5Nq+vGaksHZquNrKoOcjn1oEmVhjGHs4Ay66N5gQPfV1yFewpwoYsyWCHtD4tACO0NFmXqq4YYvn360uLTwr59blrTi7eDq7z4gEYQQQgjx/5UW/vVzEuHv8ebHa5ODi81iaWSMJJ/BtyPUbI5UzsLWLdrLy3gr2yh+gJ4uFcmV8nTOXSLYzJAZK2JPj9HTAtpVjcyJO1PadmPJtDO70Ja6QgghhBBCiH9wqmINUuPjp/WFsVRjfdnxFIWipuFf36K30cDueIzkKrRyOXQ/CFFrZTL1kLAzYLC2ye7yCvaRRfaHLrVU5uR7vua/CH0pK4QQQgghhLgpjn95ow28/NIn9k661zcpd4YM3jhHloRYSYinayjTeZLnI3QNBae+y16/jl0rEh+YI+y6GFs9Rvs9hhfcz7z24Q9s3P2NxqqkFUIIIYQQQtws5x8tL+4++/xntM4+bk6jt1jDzxkovQE02yiXIuI4QQ/DgNh10VMWcRyj9H1Kio3qxxiJjpLN7qQyqY4kFUIIIYQQQtxMft7uKLn0TrWfhc6QLCZu4JKkbDZooSguWhSiKrpBZnYeW7FR94ew2sLdaJCemUA/sUDl59/zfz5y49aOEEIIIYQQQtw0d/2XrfrUA6f+0D0xi3J4GrfeQN3uoG71yJtlRmYPYNopVFVViBodjNgkmy5RObLExE/fh1e2Se6Y58DTuztvPDVrS1IhhBBCCCHEzXTxVyeLB/60vurfNUt3LoP5oXeTesdRYtWg4hvomwNURUGNfI+w3cecnCT10yfp3TnOatbBydsYTsRb70+S3edPP/XaY1NFySqEEEIIIYS4GV57bLK4//qFx86+N0qq7RgVjQ11wPaBEtaH3g3zE7iOy7DXR1ctC+vuJSiPEG17JM0OccbHKA+hWUdphaQrs/27n5EtZUIIIYQQQoib4+5nNtt//c5UPby2jFqPsfIxUc8jHGaxShm02hxJvoj5yhuommGAodNevYbfaaAbKjXNpKylCP2A3U7r+fzCvHwjjxBCCCGEEOKmGjuw8J2dVuO5SEnIxgYTZgY7iel19mntrKFk0uimiZqEId7VVZqNHRrDOgoeacuCUOfauct4xeLqiT+TxysLIYQQQgghbq47vrLej2uVlZVLKxCbZGODtAH7rW32m9t0ly/gDR1U33HwdndRNdByBtnpERQvZOuF17GN3JmJd7/730lOIYQQQgghxK0w8Z53/UGkmMvN0xdQnIjCaAU7qxPrIe5+E3cwvHEnxtR1RifGKU6NMmztEKxvkkp0MrWpl+/68uCMpBRCCCGEEELcCnd9pXuuMHPwm1E/wlvfIu60mJifYGRqDFNRUVQFVbWsxJqcJlWpQBiwt72OH3jEVvr5/NzBb0pGIYQQQgghxK2UnVr8nporPq/ECVvr1+g7fQrlKoXpWUzLQk1UVVGqVZyBT2N3n2y5Smpxhm65tHL8h8azklAIIYQQQghxK931A/Wbbrmyos3PYpfLdOtthh0HpVjCsCxUM50mro0TKmmyVp6R8Sm6KeNfc3juecknhBBCCCGEuB2i+ZkXuxnzX5dGxxktj6ImNsnoFHo6jYrrMNyv4yQ6+dI4sePhZMzBAy/lnpZ0QgghhBBCiNvh/afznx8WUo3EtNBUmyBQabW6BI6D6jpDMpqOYthEfsRQsf+X8vyBb0s2IYQQQgghxO2Um1t4bhjyL4JEx05n0TQddzBADYtplCQmn8QMM7kP+weOffXYF+WJZEIIIYQQQojb6+4/6Z3rjS08N8wVH9Utg0LfI5VKoyqJAnGAmsuilUqr93x1f1NyCSGEEEIIId4O3vcXw9OZSnklMg3QIQpDVF3X2d1aZ6uxQxB62eV/NJOVVEIIIYQQQoi3gwv/dF7vDbpTa/tbrG2vAQmq2+9TnB4nOzdJOm2/0Gu1Zt/6R/O65BJCCCGEEELcbvXN9fvS+dxflg/MU5gaQ1UUVDOVglyGlKkSxwFxf3C2vbt/THIJIYQQQgghbqcXH86cSCWx7g+HZEwDtVpG0VTUBIXE1Bh2W2imQa/ZhqFblGRCCCGEEEKI2ykJIztqtL5rp9PEwz4hIYmqoqq6RoKCmbJRuj3aV66SUtRQkgkhhBBCCCFup9B1ihvnzmEMXVwU9AQUFNTA97HUBN33cHd2qdgZrEQWMUIIIYQQQojbK22n62U7Q7i1gxIG2IZOkiSomqbjNxp0rq1iGzaGF0KUyAf7hRBCCCGEELeViYLu+Wgo9C6t4LY6KIqCqmoqURgTqyq+7xHGIYmhyJ0YIYQQQgghxG2lKWpoqiaxH6FlcvQGPRJVQQ+DgMzYJL5houx38bUERQ1tSSaEEEIIIYS4nYauW3QxyZkZUiWLUj5NQ1dRVUUh8D1s28KcmWXgerhBLNvJhBBCCCGEELeVYaXqA8/HHK2RymTo9vokUYxOAoqmE4YJAQGlAweIEgkmhBBCCCGEuL2GYWgXFxdIDBMlGRCqECcxqmFbSayoJKaJYZqkSyW0bHZHkgkhhBBCCCFup7SVbluFAk4U4ls2SpyQRBFq6PtK1GoRKhoqEJrGe+1CcVWSCSGEEEIIIW4npVhY9dLWPXraxvE8on4fADX2PPrbO/S3tvGGzvuLIyPn7vjiWl+SCSGEEEIIIW6nd/zp1TBTzG86/e77g1YTt94gCAJURdcplkpUNJW0qbtHvrjZllxCCCGEEEKIt4MTX93dSRXyG5kwopzOoGs6KoCRzaPGMf36/tK5T4yOSSohhBBCCCHE28H5T05Ue+ur9+nEZIqlG192GQYB1PdIJTG2ZX5ha33jXkklhBBCCCGEeDu4eu3qBwxT+0LaUGB7hzAIUFNWiihTY+Bo0O9TCTtzL723/ZTkEkIIIYQQQtxOL9zXebLmDKrpYUi33qMzUiSdyaLqqCiFUbDymL5P0e/8vr137f7XPug+JtmEEEIIIYQQt8N3f6n/RH7n+v01b/gfjEYLO5tHrVYABTVxPJR2nX53j8beFpEbYGrm47vnlx+SdEIIIYQQQohbbfmfzdnJ35x9DNN8IgpcdgfbdJwm9mYDwhBVjxL89h7VaoZyPkfn6lV6GxtEu/vHzv5i5oQkFEIIIYQQQtxK0db+0vhG96H61ga7G1dJT5TIjeQwtuqEroca9odJ89pleu1dtFya4ug4BStHyg9OdlYuyd0YIYQQQgghxC01PHPlQd0LyFoWpYUZUlmLuN2mu3KFwPdvfE9MHHrsr68w2NlGLVZQrRSFjM1wY+09l/7RvC4ZhRBCCCGEELdKeHXrvmHFJpfPo+fTdFY36Cxfw9FivMBH1UxTqYyPoYQRztAjqXdQ0xkyoyP43cZDneVz8gF/IYQQQgghxC1x9ZT2sL+5fa+yNImVztKvN6EfYDkR1kSNVCaNGlk69swcE5NzaKkc+wOfwAuwigUm5ifZvXrxw5JSCCGEEEIIcSvsnH39Y6N3LI2p5TxOb0iv52KZBUqTcxTn5skUiqjoGvgR6YlZ8iOTxIUSzTiiXt8mW8piqQk/+OnkQckphBBCCCGEuJle+Ln4oV5edZOJHIOtJvvuAM3OYo+Moi8cJA4M3F4P3fc9ussX8f2QytQ8WTuLmngQ1FELWcqq/lhjf+c0jH9bsgohhBBCCCFuhguPj43tXXjjmDpfebJXK1G9FOOOjpBCJbJttvd2SL25RRxG6FYqh2WF9LfeYKu1RmZijmwhi68a+JkCpaN31dobzcXv3jup/8znr4aSVwghhBBCCPEPrZ7VXGYmTi/OTJSczZWWP2bgeS5hY4izs8vAbVOcLBHZoEOCVRxhtNegvdujvnqGfqFIaX6O0G1habXRB/7CexmuSlnxtrHwr5+TCOL/r1393QckghBCiP+f8lOf3WwDz119qHnSWNlnOGgz2Kqj1rukVYXSdA4zo9OPA1SFmMBXaLZjzMwI41MHCSOLjaubtNb32Xjr8kOvfHRyUrIKIYQQQgghbqbXfmlsrP3WlYf6m226W230yGJsfJ50foT2IGEQmiiKinL+4x9JZn/up/COLpGZWMAwUii2BWpCMOjTCcPavf95uy5JhRBCCCHE/zsrv3ZYX/zTi/LxA/H/kbc+NZ3V0klPL1moio7SDYibA9zN6wy3r5N84S9QkyTGsFI4zS7br51h57Uz9N54k9brr+HtbKHs7x1741PT2f/5H//v97iP/e3PZE6+HX/wH96v3f/f7g8e/vteu/DLldm/vWv/M698NPX2PPb3xg/9+X2dJ/++1577kPLA8+8cPn72YweLb7fjfv0Tldnv37X/mRfujx/6+17/5v3xQy/8QubE27H5jz9SWPrWO71HXviF2uL//Nr5J2btvz7Zf+Llj5QW347H/sJHyovf/Cnl791XdP7+0v0vHHOfeu2XJqtvt+M+++tz9ivvTR78/nv+/vfpdx/kwefvGj5+/mOH3nZj/ZVH88e+/872Uy/8dPq+v+/10yeGT/z4Xd4jF35rzn67Hfvzv6jf/7VT/SdeeHzmJ87pb/3qTPb7d7mPv/4zpbfnufGh9Mmv3NN+6u/9uX4+e+Iv74seevnxybfdeHnjycniKx8w7nvjPv7ep3y+ek/wyAvv7D556VNjY2+3Yz/3S+mTf/Xu/hN/+9GfPP+d+615/dn7O0/+zc+FD6/8058cT7fbj34uffI7J3tPvPZYdfZ/fu2lT01Uv36f89jpx2pvy10mL75He/CvT7p/73f0Pffz2RPP/6x2/xsfKy693Y775X80Ovb6O/1Heq+e/nvfp6+cDB975QPuY+efnHr7zWF+obT4wqnw4e9//CfH+tl/uqB/78Dub59+0Dr1dhwvr/x8+uRz73Iee/NTP9n19MfHxr72zs6TLzz29pzDvPZO7ZE3T2o/MdZf+vUFPdpvLiabe/ReeZPBi6/R/dtXab/+Bs1Bk7CWQ0FFTxQFI0gYHYZ4fRdVcVB8FyMYougqe53u94vH7n0UzK8/d0/9M5Ozky8f/pr3ck3zsnvnf/zUXy2k6tWFuR/c813ltj+97LWfdR5rvnn5IW/VapfnJk6DwdXfnNe3Xjj9VBDF+ti77/68qUQdJfZ+f/OlH335vy+m6xN33fPZE1/rnrudx738G3P24PWzj3VX1+7zrhr9+buXvgUxV36luHTpldeerM1MnK6cOPr1/l5jo3P26nfbl579/KvzpZXMu+76/JGv7OzczmM//4na5O4br/3G+t+cWcwUck+MTh08DG1e/0jq5Orrbz6+sHToW3d+J3luIqe5y2/96Le/PgejtYnT9/24+ge3/aT1EetU4+L5R1pvBtl0obL83r/c/zrAy+9VHlAuv/FE6b33/iFV5YyZC7+wfvrFz35v3OiPHjr67LHng+dv53Ff+fScvXfh4sN7axunmueN/ujk5GnQb0z+79755/huYeGed/17IvO0tnrm+80X1459/0CqUb3r+FeP/3ln+XYe+6VPjVd3zrz5+PK3zh8bN7NPjZ84fA8MOf3rpcXWc68+NTI98+KdLxrPlnL5Df/y1l/V//KZL//4+MyLqXuOfP34n+ze1rH+2q9kTuyffvPx7vd7U5Vs4fH4ziM1/mbIlZ9O37dz/uIjo3ccfObgd72XO7NGe/PS5W9U/uzcM6/ds/Dc3a8WP3vbJ9K/4D+yf/XKg403BtXa5Pzz7/3yWh/gO+9vP9XZ3j5x112n/hBDWRmJ/C81X3nx889X9X7u1F1P3/3N4enbedwrT83Z9eXLH+xub5/sXIn1yamRcwBXfnOyeOn7P/iXBdUc1O5/z79TNP3c1eeW39j69tmnfzBXvF45+c7PHvva3uZt736s/1Tn639zslEznqq888i7eNFl+Yna5Np3f/jbtamZF+96xfp6tFQ7p/zNjy52rqx8+aWTUy+n7rrjmROfu73n9TOP5Y+1X/nxU62/7k4VxyYeyR86moMWb3zQuG/92uUPzZw4+vVj//7a6Rc/aHyue+Ei62+c/fIr77vjy/f+QPvmbT+v/2z6ZOOttx7rvuIWRxcWnrrrmbpy9akFfff1Vz7dGvRGF+65+/Nqzlp1hp2vNn78o3/3nbmE6h1Hni3Ozby48O9v38OLLv3WvN48ff6RcGPnRHjVz44fXfoWwMqT49W3XvjBv0ynC5sffGPk301UM7vbP37jjdWzjWe/czi7M3b3vX9455cHZ2539zPzW79z5cUrFdtO/fbCnYePc6HHykfKi863X/nf9Hcd+YPDfxO8mJouLLs/+vEb229eevpv7p44PXrq3j88+ke394FRZz4+OtZ/6cyTvb9dXkxNjjxZzsznoMW5n1YeuLZ88eHK+9/xtKVzZrpa+bebr73xh9+YVp6YXDr+zD3fDZ+/3c1fu58HW2+ef6T1YzXMHZ779J1/sqGs/JMFff3VV58Kw8Cev/sdT4em2S5Fzud2X37+6a8vau70O+78cmV69sWF/+ttHOtPzWS7r7z1mLu2fbK3qbnFOw59C+CtT5UWN1bX7vv5v8k9rVxduy945aXvpzMqihahp3MokYEK5A0T1+vjkqCc+/hHkkM/fz9eMU0w9DE0HT0MCPotoiSmr5mYs0c/eerF3NM/vGvtX2VCt2iOTZw2soVV7/q17/d6A/Z2dl7UMunGxOFD38yM1pYP/dmt++Ve+Ghl1lvbPrWxdvbRYW/7xMHRuaVMqUAwM/KoNTZyzrt45cG9M8u/r+sG5kiR8t3vYNjsoG7t0tncpb/fejldrS0XDx/9Wmph8TsH/3TVvVXH/tavmPftL1982FtZuy+ud+47cOIeMGyMe46O+636knPp+v27l6/+K7taIjMzTnHxAN6lDfReiyvnXgfNWK4dWPx2fm7xuSPf1Z+9pYPwfTzoXL/+vrWL5x7NjJaWJo8coK0pVO66s5asrb+nubJ63+pb5/75woHFz+bnpl5WJ6rL/Y31F/xOj923LqzaqdrZkQN3PGPNz7x4+BvNlVu2APjVkcn+1vqp/fPnPrZxeflDh+84kq2MjeLlix/LzMz/YLi89p7+9ev3d9565TMz77mXZGGKSNFQ1uq42202li9vmqOlK6UTS98qHzz8jaUvt2/ZouDiY7XJ5tbmqcYbP/idvuucnF86immlSS0u3mPouutdvfLg5sqlDyme88DU4SUGCwvY3QHe6hb+fpPO7v656szc89mlA99WFue/t/TZ9f4tG+s/Hz802N08uX3+7GNJFB9bPPou9CAmdepEzXWbi81r1++Pzq79nmVaT5cOzD1vHDr4bWf58obW22d7dZWON1wZOXjom8XZA985+p1bO1E690EebF+8+PDOtZUHipnc0uzhw6iqTvjTJ43h8ur95qurH4y39v65OVX+39yjo+fUg2Nnts+eP1te69F+/dJqr1y9OHvijmeyRw5+6+B/vXULscu/Nl7tb137wMbZ00/29rZOzs7PVWu1EZxs+ZOZ+UPf6ly69gH3/GtPdhuNB2fvuAN9chIVk2Bjh9bONpvrq6uFydGzk3cc+boxt/StQ39y6ybWb36svDi4vvzQ9huvPek43onjJ+5Cswz0Q/PvMjToX7784MrrZ/5VQTNJnboLY2IcY6+Ltd1i+8xFvF5vs7J0+BvasQPfzh1e/N6hP1y7ZWP9jQ8a9/VW1+/bvLLygdHA0GePHXtgMzeg+L4TtXi3vhS/tXp/cPrqv8lXRj6rLE0+7x+bPG1cvH5R29xnfXuD7U5nc+neU3+QXzjw7UPfuHWT08ufnLWj1dX7e9dXPrB15dJDdso6dvDYMbx0GvP4CcN78/wjYac1t7G68nupWpGRpTmMcploZQN/v8XKysbLWrGyUlmce752+OA3l/7r/i1bRF752NjYYG393vbKlQd3VlYenDt0YLE6OU40UiUereJcuYR3/hwOIYXxUTIHl9Bdn3B9i729XTbXV6lNjj5Xeec7P2vPHfzW4T+6defGy5+ozLbOv/LpzeWVB72Bd+LoHXeiGBr2saW71CjW/QvLD6+/deF3zXzhm+n5+e9Vlxae665fe0Nzuly9cJ7B0K+PLh1/pjJ/5Ou5qfnnF//zrZucXvg561RveeXBtbNnH1syc239/qP3begBs3feOR5u15fM19bvaz13+t9Y9x7+g/7J2edGxkbPKa9fuOL2+ly5fIUI9fTMHXc9nZ2Zevnwt72Xb9nFkU9OVION9VN7ly5/aPXC5QcPHVhaLI1O4OVSpA9O17xrq++Jrm+eunDh7O9MvfsuCpUaKbNAsr7B7qDF9sr5VSOT25l8x92fz8/OP3fwK61bNoe5+CvV2fjq2n3ti5ce2t/eOjl6aGEpNT6KPjuNVcijXLvItfPn8YOAyZkZ0hMTxFGEu7fNxtYGg2aTbLnyXO3Ee/8vxdm55w/+p+u3bM67/EvmfYOttfuuv3buEb/TP3XknruJMyaZY4cOK4PBWP3S1Q/EnUH1vpUjv/nWB5QHhm/86Lt6OmaQjtFSOTKhTXoYomYsFDWg/p/+241FzIGfvw+nnEaJFdQoJuz3iLwehmnRRIexhX/2gR/V/uCHB87/sdrYfao2M0Ogm3itBrlyGS+CnWurNPZ2KRRLLw/KY2cmDh9/5t7vJjftEVKvfiB6qHH50ofCVmNJG3gPuHrIgRNHSUcakTNAXZig3djFGPqEq7s4/SH5iQr+yBiqbuPv16nli3jtDvXVNQZxAqNjX3bSmZ3ZO+545u6/dG/aG+pH7x4+vvXWq0+lQz+rR95JM5NiYnEWVc8ROgmMZGEwIN5t09qt0+y1mL/3LkJNI6VkceurpKpZmivruFsNlFhfjrKl65mFQ9/Knbj76SPPrLVvygD8RHV27ey5R41We1bZ2jqZCsP70tUq6YVZIiXENQx0U0PtNOms7RC7IbaukZ2bQKlmabXbZEMwdZud15YJez5RLvPc0LIGtaOHv1E6ceLpg5+7dlNOwG/8Uu7Y6puvP2k39445e40Hc5pB7fBhshkbJ3TRchmUCOKuQ2dzA1NxMLNFzHQOvVJlsN8gY9kESczK+bOoYYg+MvJNJ5XbHVs88O13ft9+5maNl1ceCB7euXTxw+bAqbrt9sPlvE5tegoznSWMVeJsjqBeJwo8/L0tol4He2qc/oFZbD9CaXSpWDnc3Sb9jW2aroufz71YmZ44XTh+19PHv+HctKvtf3tP66nW6sqDxrA72WvsnJpcmGN8epqwpxNlLIJSiri1j9/soe118fsu+QMLhJU8rtvH9NsUMjl61zepX9/ASBXPKLWxM/HoxOnKXXd//viXb85k481P1iavv3L6KW/l6gPjUazbnncqOzGOOTlOrCQMLAO/YGO1BvgXNqDnEOsJ2TsXScophu0OxWGCpqTYP3eJMAjoasnLw4zdnnrHiS+fejH79M1q/tKHzfuGF84+Rmt/yRn0HnQGfQ4uHSQ3Okav0SIzOok3cIiCEHvlOu1+l8zUKNSKkM3h1juUTBu316e9tkav12NYLrwc5Yurc3fe9fQ93755i8gX3zV4vLF65UHV6Y/G/eaDpWKRytQcahgS6yrmaBWv3STsdOjv1cm7PsPD82i5HJlhhNrsYWQKBLv7rF69Qlgu4peyzxojlYszS/d89o4/uzmTjUufGhvbfvPM44Od3RPa0C0Gne7D+XKFyYUllDDAMTy8vE7SG6Butkl3PIa9Ianjh3CKFtqwTypR0NyQ4V6L/bVNAs04E4+OnEsvLTxXOXHs68f+08ZNOa//8NHMifZbFx6xduqH7WbrscjpUzp+mMzECEG3STI5Sb87JOcrDJcvEWkJg8Rj7OgByKToXd8mn6mghzHX11ZptVuYhezzaqW0UpqZfXH0jju/vPgfb85E6czP26caP/zRZ7xOdzKtq/cnccj4wQWMXJqB76ONVhjEIen+gMGPX6VnaGTKRWrTMyiWhbe1h5XP4jkDrl98C8826RvWObs4cm7uznf+4T3fjG7aBdm/fU/vib0rlx7Kh37G7rceClSVqaVDJJk0vcAjXczhN1rYXRd/p4Xnu6TuOIgFxLZB3Khj5fO091tsrm+SoGNmss9p47M/KB97xxdOfq2+elPmAb82Ud0/89ZjvfXV+7VWazbtB6eKo6Okx6cJzBC/YKLZFv5+G67uowUQ6QnRbBWtnEdpd8hFCgTQvrZJb3sPxmvf7Oezu9rsxOmpu+945vhnt27K57DffCR3bPe1159MN1tzcaf9SBwG5A8dpFAt40Yhw1Qa3Q9JOQGDa9cI1JjI0hmdncIzTJJWHztlQdzl2spVhn0Hs1B5Pk7nN8ePnHj6HX9z83Yk/fWDwcP1K5ceyq83lspd5349icjfcRhjJEfTD0iNTBHvt0l112jV92gNHKrjE2QLZfSUTWt3l1K1zKDTZmN1FTfUsErlZ/VK7ez44ePP3PHfbs4FkytPzes7b5x+auvqpQ+V9ztTqSQ+4deylA/PE+gKsRdQTudRQ+hdW6edJF++f+OuT/z4/e5j0fKZr6q6Q9vw0ewsZaNE1tdw9Ri0iOEfP3tjD4oRKSQeqHFC7AUw8EnrFqaZwfEV4sRuA5TM7M7a9dPUpubRDIPYtGg2W2TyBcZrY6RDBcOPT2X73inn/IXwR+8YXcwfO/7MsS9u/p2T71tPjI1FnltQs0b/zs/tb179zIIOCUmS6IqihmEU2J1Bf9Q0dffEZ//u1ZyLnxivdi9dfDjabCzlE+fTrjokO2KSHptDiQ2ioQ+Jir/fwk6l2Dp/hfHIAkVjsLbFyPQBnETHB5x+A6uSoUwZvdtj4DQfn8zaDN98w37+p6cXRw8fefbwH/3dK3hvfWLWDgOnYmesjp7EupYzXCIgUcM4Soj9WB84bvEdX/3JK64vfzx/rP3Kq0+l10P7oB/fr3gh5DNok5P42Sxh20GLEnRfQQmGdC5doDZ/gH5zm3Z9m9LEBH6nSXM4JN9TKU9Pk+g23n5jKUycJbW/++D+68+P/vDnDnznPX/1kyffNz8+NqbpmmuZuquaWpiEflYx1DAOYj1KEkIvtL0w1u7+yk9eQXv14cyJxssvPZGLw9/2u3tY2YRstoBZzhO5bTpOQGRnKBUy+O0OjetrzB6/m73TpzGKWYyMSRDENLpDxlIm1cPzdFcukqjuA1lDw2rUH+699Er2rV9cfO6O//vf3d73+m9NVP3+sJI10w1L01xVM1yShIX/27Xw6m/O2gkwrPcmNdPqHP3Tn3wIxWs/HT7cPfPmqUmU3/aGPSrZLLnSGIZRIPI8+n6EpXrk0yk6zX16rT1Gjx1g+8I15o/fjddx8MKIvt+iPFZj+vAivatrmGHwUN5po2yt9l86NVrMHzzy9aNf/Lv//5ufmte9oD2by6U6umm1VVUhSUABwii0fc8teJ6fzabsztIf/90r3Rd+Y6rYunTukXBr98RI6D5ph0NCSyE9NoaezuL4Ed1mmyI66UKV1dOvkLd10rqFs7FLev4AVqzS9yP6XgezWiQTB6jtHqDdZzR697lvvBW+cv/s5L3PGz9xJ+/MJ2eyeE7B1s2BkTdctFgn1l2ihIX/eC28+GvT2YHvZ0/+PWP9rUczJ/qXLz9k972MGfiPaIbJ2MIhCmM1Aj8kjmJ6oYs5GJImobF8icnZOYb7e2jNFup4kZ5l4q8FGIpDbmqSVJTgdLonYrdxQvHNJ3bP/GD0pV+Y/8G7/pKf+MPx6q/WJtO6Fum67uqG2Sf0s7GuuXECkeNn4zDIhEGsn3jmJ+8Cvvgh9YG9F/72kYLrfVo3YjKEpGtZtLECXuLixwlKrFDp9hi0GrQGO5Smp9k7e478YBzDDwj7PbqhT7ZQoHbnLL2r1zA15VSkKETrq6s/uqM0lV5a+saJP+/9nbF++rcWdMUPbBs4+sfr/aufnrUx1DCO/ayigucqhPXQ1rPp+tEv/OQV15d+avB4/dWzD9TC7pN6OKSQSWFOTpIpjxC2XRwHrL6H5Qfs7GzR7nVJ1yqsr6wwN3UfGCYD10V1ffKFIqXxMQxdZTz2T0WRc8q7dmXnxXtrs8WjJ54+9id/dwH52q/PZAnas+lcdvfwH23Vr/zWnK2oSrj4+9fC5d+cziZxrA/7/VEznds9/sd/d0L+1seniv1LFx6msX9sTI+fUJwOA8ukNDaOnagMI/AcF0VtkSqXuPDqGcZGJzD7DZrX96kuFdGjiK7nYBKRmqgwYcR0Gm2ijvtwWfcfbr/xKm/8zMyLuQOHvrX4H//uxZI3f3WiqsSxbthGXy+oITGoaOHC799ofP6pBb3tuMX3/OlPTq7O/lL+WP/cuftzrvNv424DW9eIyylGJgtEWp/GoEvGVygoWZKOw6WL5zh49zvYbe8w2WtRsEboK1DvtygZadLVAmPOgDiMTzjB8IS3u/n4xgv7x05/9PCzJ//iJ3c5vP6J6qxl6J5hGH1N012SyEZRQhSFOIx03/ezbhhZd3/pJye1P/xA/4no2uZUNXZ+1/M65Cs2ZraCVkgRDtsMlIigtY9hGvi9Ls72JiOHDtC4eAXlwCSKZaDncrj1BulCmrn5EfJbEe1W+/6MZ9+v7e09ufejl6pv/vLSN+78s5/cyvriJ0bH7vvSjfPH1d9a0FESUBIdIt3vqqHbG4xq2ezuHX/ydxdBy78+Wdw7d/axxpvr91WM6PGhHWHaOtlSFS0NUeISBC54GXRFoX31GlUrRV9TaO7sUxmfRCGg1x+QhAF2tcj88aN0168Tu4NjWugfC5cv7Pzo3rHF4rETTx/5L//TePn1kckgcIuZXG5TNwz3wB9cd1c+M68rikKcxLrveVnXcSqKXVh9x2f/7u6ON39tbKz5xmtPKpvNxYUkeGzYaaCXCkxMTaEYNk5rQKKrqL5CWrdZv3Ke/Ng0g+027tpVJg8dJfZ8Br0AJXTIl0ZQ+h6x5xAMug8YTvuB9luvWq8+MP3De577yQsO5z4+WYz9yLZHlFCz1ZBEgVANCZVw4T+uuud+faLqqWp49+d+ctF85sOZE83XXnvQHA5+r2rEOIZPoZbFmi7ixi5JzyPoNjHHR8gFCed3rzN/6i7qr7zBVEZFzZjsRwHtRpPRYoXiwWl0PEI9fijuN0n30k82f/TS6Eu/fPyZd/3ZT27zf+Xx0mLO1F3DsjuqrrtEsU6S6Gi6GwSB7QdB1vVj7Z1f/smtpD/+ueTBwaWLJ8eU6LcJh0S2ip2vYJYsAtWlPuyjmDGVxMTb2qHX3GXy0AF2l5dJRoqodgZXDxlu75MeyzM9O0dnaxui5P5oMKB37g1+dGL8ROnOu54+/Kc/edf6jU/UJi1TiyzbbhPHOobuEoU2seb6Xmh7nltQ09ndO/6nC7lXnpiodq9ef1+0s3c81R08ZRqQHU1j1MoomQg/HOL2HPRkB1u1aa2voiuQz2TZuXyNwz+9CJ6Ppdk4HQfTzjM7s8jw+mXiYedhUubD+2+dLn7/XROn3/9S/vM/MV7+8cik4wd2yk51DN3o66oWAsRRpCuqFka+l+00B6PvfOYnd6Wc/eXSYuP0yx/ODTv/dhYfW7fJmDbDsQqepuCHAUoQowxDkqHP3rU1MidPXGED/CTWa2YOPVSo+Dq+E2PqIYpl4uvgqREaoCeAourEQYw3cIn9kLSVxjRVohBSmQKdUA0BVDXdMBKTfrtLrpjHC0I8N8LQAnKZArWZEv2z57EyOqly5dNOFNO8crn/wkfHzrz3L24MyNd/qbTo7TfGnI31F1J7+58/XQrt5p+c1m3bfiwMQsIkIjENYtvEmJr45Fs/V1q9469unLjP/8p4dXhl5YOm63wuUyowiHtEbkh2qoaZzrC3F5L3YxRVwcrl6To9vIGHmS8SBxFOt093c5/87DydwKer+zB0GD00ibu6wWBtj1TsU82PfHon6H26funNf3zhibnnjzx945fzyi+kT/Z314vu5tp3nVadkm1gt/sYqgGKwdDz0fIFwmLxn5372cmXqRRXj/2P7SNnPxI9FG9eXTTS8Wdyo1k6/h56kiIze5AkUBhe6eBEHlrVIKXb+IMuehKCEkHk027VyUyOkC6YWL0MfheGKY/c4gxDO6G+sc5cdRLLUH8nqq8P3nxfeunOH9zYi3/hN+Zsd2PrpL9ydTbc2/1Ss1EnZ9vESUhIQFxKsdcbkKuOoOSLv/nyhydO50ZHzh397I0F3I8fUh8I6luzo5b120omy8rmMumxMuZUBb83oN/xUGKTYmkcgoh2o0VKtVAjBUsxcTwIQhXdytDxh/hRgFWD3Dun6G63aW82WczNEcfq7/tX1n/z4s/O2of/un0a4MqjuWOcvXKitVv/0rDTZ9gbYqKAZjx9Jh/aw69drAf93qfdiTG0uYX3Pv+zyon7//r/eQfwh+8PH04ajQNTpervhN0Wu4MBmblpjNo03n6Xnj/AS+uU8yWcfov+1gZp3UYZ6Oz6Pmrco1Adxd920PYd/P4emaPzkLfZPf0aC4UaZLOf6fQ69Feu9N/4lcXv3fVfb5zA3vj4ZLGzvbNoXb38qtNpQRThhyGZbJZ+p4uiqlgpG8Oy0KemPvbCR2defO9f3FhAvvXJyeLeysWHs0n4uVzKwmm4qL0W+aOHia0Mnd06Tgh6Ko9hZnDqTXrNHrXRCppikrQH5LZCjJyN04sYaAFYOpm5ScJOj/ql60xUR8mVC7/tDFqbP363/unU0uFvHP+TGyf/H36ssNS6evEkOztfSppNiv9j8UWsounWZ19IO8Xhi1tnhobRP/dzc89H+dzunc/s7Fz93y/ozSvnHnFWr52o2ubvaLrNMI4JgPzMIp4zZH/YJ+UE+KUMWs7Eu7KN7rpoloqqQXdnC/NADW28SHDJoOcHqGUT+8gc3tVLDLstJlIVMhn7d/zdq/qrH6hO3vO9Gyff80/OZP3trZP95UtLQaf9x/7uPtVMFqfdJKxkcVMazVaX8ugYpDL/4rWHJ1/OjE6cXvpPNybkzz9aXMpdu744HhufTsby7PZ3UcYKqGMjDPs9+l0f31cpjU0Q2CHdXoNcnKAnYAQ+/eYuhaUFLCPNYL1L5PtkD1ZI3zlNd+U6OAGTk0tPRU6Me/5af/mD8/rSd3pnAC79YuZE8Na16mBz87uBH/zrVwqdufaXXtcTTXk8JiCdtqm7IXFulNzszPtf+FCm/d5v3bh69uaTU0Xn6rX3ma3B2OGxiSdp+eztrjJ69ARKvkqvMcDxQxR0FMPGD2I6+y0K5SyOoeBqOn7TJRunKRk5/G6XXtImPztJNJJHf+UV1CiiMDH7mWHg01pebr/2+MFv3f3lGxeovv/hwpK/sV3NXll+odtqPfNGVXf3/svLRcuy269VEt37s7fcRFGeNCybweLch7/3q+OnP/DFG2P99Y9PFnur199nRP4XKrUSzuo+ceRQvvNeFC2NW2/Ra/fIVCqYhSqdnT0GzR7KuEUYqqh1B2ZUVDNNn4Qw6FMMNEqL8yTKBtG1PdI5i/RY5p/3d9foeIN/fP5/d/jZo//pxvv0x4+mT/ZWLpzQOt3PObvb5C2dwI1I66nPv1qKdSXRXOfZ1bZVqqwu3z9yjkxuZ+lbu8sAr/6ifWp4/drJQhj8Bw3Q0hZB6FE+fpigtUev0cf3Y6xqnsAI6Q1bGIGH1nfJxgqtvW3MuXGifsKg28e0QygUsE4s0rhwkSgIGculicz0bweXVhvf+/m58AP/PXgR4OInJ4vx/t6xxltn7/N7vd/z9+oU0mkUIpQkIUoU+mGENTZCnMv+5rmHxs/ok5MvL332xiTpb39GeWB0qzOl2Pq/8W2NNTsgNV4iMz5Kb6dOjI479LFzGXRbYbCzQTZtQORjEdPa3qKQslGigK47xDM8ymMVShM18Bwid0h1coYwTn6vc+16+7UPz+h3f+PGov3sB4snjCTRO69cvP/HI61J3XeyjT95rZhO248N/SGaoVAPY8yxccyR2qMvf3z03Kmv3Pg7fO7J2qR79cKDRaf7x+nxMl2/zXCzRWGmhGFnGO43cN0Aw0yTcmOSMGbYHEA6RaxEOEFIo+9QHi2i5/I0u03Ydpg4tEjV7RGuXEVXVOJs5jP9oUPv/MX2G48tfO+uZ26M9Rd+uTobL6+dMjbWv+o6DlEUffn1EcNtfvF1W4HHkyhCt230dJr+8WP3/PiJidV3Pn1j8fvjx0Ymu1dW7h81zX9jlcqEvQ62qRHfdRCv5RLsNIidhFwmi2nr9FsOHc8nV0wzaCikXA8/CDEUk1BNM4w07NaQwsw8SX2HQW+VVBJSKKR+p9fe/l9f+WA1k1068vWj//7G7/zFDyoPDNZXKs7W7lfN9jYZFQInImtnCRPjs6/mIz36ztpOlEo1lt83cVorlq8feHZ3FeDcA9ap8PLqqRHT/j2lanKtcQ1zPIN+eJpWswH7BlkvQVmoMEjFKJc2MRwXNY7JhjH9foOUOUI5lWc7adDo7lGdnCDz7qOsvfkWmUSjhIoWKL/bvHRt928/shD+1H+7sfC98Gsjk26rfnhw5sz7BkP3d6NuD5OEOAxREtDsDL0kQq2UUcqVT7z5C5WVO/9fdtO8dZ/3iLndnTJM/d/EkUJdd1ArWfLz0zh7+3R9H1238TIGaj3Ab+5haBCrMcpggN/v4E0VcHoBkNAeeIyPjFE1M7TePE+uVMIarTzuJMHj7Yun9dMfHTtz8i9uLCBf/8XiEv6w2nr51Q8a7vB3064LKJimhee4BDoEGZOhnSazuPgz3/uldPsDf37js4iXn6jODi+de9Dy/T9ODRL8UKGuDkgOz5MYKsP9JjQUCnaBVD5PJwzxe30ypRJhoqCj0d/cIzsyiuvFDByHXF6lUKlhj3doXl7BHhsjm0l9ehgFv/n8+/xHijNzPzjxxRvj9cWHMif0c9eO6VtbX3LcADcIcIcu+VKR2PNwen2MVApvfPJ/+cEHZ7OFWnX5xJdu/C19/aOlRaVRP5yPwn9rWTbZKKReTJGeP0A2SLAu7dNzh2iTVfy0QT8ZMtQNipExADCbflbRLWIroGcZOH6E1QvIDQNysUHRMqijoLz0sfcm7/zwo7RDHb/fJ2WlsK0MMTAMI+JUmtA0/tn4qVN/2Pyr7/6b/vnz/1zJZxm/4zCRZdBuNVB1Ha/bZ2JiCtUPuLZxhcJIFTNTgHSGIFE/kRkdP5PkC6udvf1j+UL5JYUEOns0NzYIXZ/Y88lk0vi2RbpWIl0r0Qtc9CB5r2WPnNN9p+I1Gwc8d/hXpgKx77K3eo1asUCuNkJ/+TKGnaKXUjCMiHyxRPvMRaK1Hapjk3QTj67fwk3VOHD3fTDoEQ4GDPp9LNsiXa3Q67RpdtvkahXUfAUrnaHf7/9GafbAt6MgtKMYNDt1RY0Tht0e7f198t0WXd/HKpVQrQxWsYBdKREELv1u+72V2emX3evX7zdavVFF5UuaAp4zoLW7w/jkJIqm0li9TjaXRU9ncNI2qSBg/8ybGI0mtaXDNNbX8FI2jNYYOXwEZ3sb1fPpDfoUazX0TJZ2o4ETJ5Srowz9AF23PqkVU43swvzzve3GopIuvKFGClai0KvvEbYbmKFLp92kU8tRro5QrVVI4ojI0PE6rZ8rTU6c7ly98kE/8IpazH+wPZVhu40ShFRqY0SDIfXdXVK5LFgm2WqVaH8L5+yr+CTkFw5Qv3gFNZ2ivHgAvVQi2N/H9X1CN6E2NUWowvbmJqqdIV2poqUzDBz3H+drteU7vh0+/8qHlQf0jPXdJEzQhh5hu0fc7qJqOoHngWWRq1bQxkfpeB5qEt+jK7Gey2d2OtdWHlQCp2jF4e/h+2xtbGBXp5ko1gjX6/iDIUHRxjViKqNVds5fwl7bw0ylUSsZWnt1QjvL1J3vIE5inE6Xbmcf3YTx2XF8x2VtY5tSbZR0rkSg6gSJ9snS9PzzrmF1Bu3uZC6VPauZLuGgTnd7E10JMUnwBkOSlE1pYho1VcBJIPK0e7RMblMHnJ3Ne2PP+4aWxKi+T7/VImfbZLJZ/NVLKIaGY6rYpoldqdI8c47B5h6jk1O4nQ6O5xFMTDJ95AiDZoPIc/HjCMUyKVWrdBsNBv0+5fFJfM1CNTRQ9Y9lZ2Z/0B26BV1RSBTtop3K0Knv41y7DlEMxKhpGzWXIz8xQYROEMdE3d497/q2c/rMh6xTsdt7SQ1DlDDCa7Vo7e0xNT2FZhpsXbpIYWqaoBWRrRVQ8OmeexOn22Fkdob27h5RAPbEFKWDB+k32xCE+P0+xUoV3TDo1vfwbQOrkMOwLbDMT/iW2c9Oz/2gvbm/ZOfLL4VhhJlA2Gsx2N7A77fQMgUiI4Wez5IfreHFEYZp4Xve8Xu/3j331s9ap7yBW4z87l9lsgb1rS0sP2B0chK/P2Bve5vsxAR6mGAXSsTbF2hdvQqxQnlukd2LlzFyWWqHD6GWS7RadXw/xOoHFEdG8DWF9vYeer6ImSuQmAahoj5qV2rLdq284l+7/MFhJfsNO2syaHdRd1rYLRen2yO2NPysTmpqEbM4ReA6xApTtmkMbE2hu75+So2jv9LjED1RuL6zTD5tMp6uEe41GbgOUTGDmrXJZbO0Ll5neG2TfK2Amk7R3N7ALBUYPbREHIX0un2coYOtm1Smp+n32+zv1bFyeTLlMqFh4HnRPy7NLzynmFZnMBiM6qp6VtVU3PouyaCDEXk4wwGqomDkS+QqY6i5Eh3fI/Kc9xbL1WWCINNaW7svZehfUqKIfrOJ06pTGx0lZ2VoXbmKlrHx44hiKY+qKmwtL0N3QLY6Snbosek5KNUiM8eOMOg0cIcubqtDsTJCpliktbFOwx1SnjtIOpNh4PqYdvrDucWD32p3e5OGqnlJFG4rKZug04YLr6HniwTENIddKhNTpEYmSMLoxtXY3fr77ZnJ097e7jGn3VjSVeULiTMkHg5xWk2mJiZI/IDB7g5JvogTRhRLFcwEtl57DTUMqYyN0NzZJtFicpMTpA4eZXdrh7ShM2g0qY6NYOVzrF27hp6ysWs1FDuF5quParo5SC0sPNfery+lMrmzCQmamtDb3KSzvY1KTD5nEyUq1sgkqeooTn+AbqcYDnqH3/XNzvKr7w8e1pPItuLoq7g+/XqTYOhQm1vA39+n0++RHqnh9zqUpycJrl5k7/JlUrZOdqTK3uYaST7P2KFjWIUq+10XtddGc4YUJqeIopDraxsURkawy2UiwyJU+FiqXFlWc7Vlt1FfMrOZN1RFIfIc3P1dnE6bYa9LNV8gJsEZmaYwMcJw0Ma0jcOqBhmUcHh945ThK1/S3QTFVLm6fpaRTJ5SdZT23j5O4GNns+RtCxWN1u4enfPLzIxWGOomfq9LP5ti8h13ECox+zu76K5PycqSquXouj12d+qMLxzCjxWMdJ7uwP3E2OEjzwaOlx26fkY1/Cs5E7xWk159CzyH4XCIZphYxRpKpkCuOkKEhe96dxVKpRWv01oc7O0ds0z1S3aS4A0H1Lc2mRobwUjF7G3sgJbCNPPksnmUKGbz0gW8VoOx6XH6jSbDyCBTKzO6uIDf69Hr9/F6A/LlEulSnvreNl03pFAZI1PMM/AiDDv1C/nFxW+3e4PRRFUi1Qu2jXQWt36ZaHebdGShegF9b4i9NIsyXiGJQszExNhrfVidWfzecHfnhNduz2Ys40vx0KXbrJMM+4xOT5J4PntbW6glCyOKKY+M4u3VaV+5RuQMqc5M4u7v4bh9ioeOoU0fo7u9RRiHDPb3mDx4AE3XWF+9TmCajEzPkMQGSWJ+2MjYnTv/Jnz+xZ/V78+Vy98PnSGGouD1BzR29ymkfJKgR8+DkblFUiPjDAZDTEM/HAw6U7lKfmO4du2BxIt0A/X346HDYK+BnigUx8dwHJdufR8tnyX2XKrTczibmzSWz5LKmuSKORrb28SWTe2OE6iZAv2NHfRYxfc8qgcO4A5d1q5dozIzTbpaZeD7xLr+sXSxvKIXSqvdve0TqVL+u6YGkevQXl8n8ALo9hkplGhhUDxwBCOVwul1iDRtTtc1L5vEWm9r85Stq1+Lw5jE2aOzvYFtpShUR+hurOOpKmY2g+0FpMpV6lfXUDeukq5U6CgJg14PLZdncmnpxg4INyTyAjRU7IVpmtubtNptRmfmwbRR01nCOH40Mz5x2gtCmxgiJbmY1kAdtGntbOD2eiRArlRjECpkR8dJ5WoMvQg/cI6/+5vdc289lDkR9tuzsT/8RkZXcPsddjY3WJw7iKIb7FxfxchkKVoWimkSaTrrFy7Q2dvnwHt/6jezJ44/s/ujH/2WGfO7BqApCt7AIfB9DMMi1lSKaZv9L/wpyulH35fc9ZGHcHUDxY8wVANV1UgUg8QwCDSVrudQPXaU/t++grq1S5xLoRcyZOZmcNwh18+fJ2VajE1OYZfK+G4bZ9Anlyvgxwp6Lg+KRrvbx5hfIPIjcpaNH+ioUULQbKI7Hp6qkpueIDFUosijryoYGnD5EulCjoSYwB0SOQ6WCkEUks0XiHo92pdW0PyAYLJM7fgBvI0d+ueuknUjYsclyJtE5RQdX6U8tUAum8fb3aO1s4uq69QOLaLmM7Qae2i2ga7YaLqJnrnxWaFmb0BmfgEzAs8P0FJpPMfB3lpjGMf46QzlsQn8OCRWFNw4IK0ouOvXKBRLKEGC3+9hqAqB65CYBqmUhbe7Q7tZxx8OqM3NY8zO4qysEG5vY/kew06HdD6Pb6fYj2NmTp7E1DU6ly6xvbFBbWaWwtgEajpLf69OJp8jQiFCA9UnMQxaisFobZIoURm4Prapo/kO7auX0JIQffoE6doIoTMgVhO0wIfARxn0UTQgiYh9DzXq4Q6GZDMFFDSGm5vs7u2RyWUZmZ9FzWdpLZ8j3dih7/u4no+dyxPFCkYqQ+H4cQhDdi4u43cHTExMYdaqJJZOq17HymRR7BSqbWOZ1iea+/vHjJkD34l04/thBHqUkEplCFpt2utbpFM2mZEaajpFPwGDBCtls7VymZmRKtGwD94QTVdpbm2RqFCeG4X9Ds71Ldx+F6WSpXTsAMNhn87l65SaLrgOrhWhpfK4sUp6ZIzcwgL9dpP66mUi32FifpJUsYTjhoRhBAmkyzUSdJIwYb/dpXr8TnrdDpZdRFU0iDyG+5sY7hA9lcIslUnMNEGsE4cxejahd+UiadtGVTXUKEJPYgadDug6hVoVf2+PwfmzqNkUSd6meOQwvdV1hlfXSQUQ+wGappPYFk0loTg5QXF6msHONhtXr5IpFJiYnUG1LNrtFrZukJgZDMsEwyKOI1o9h8yBg6iBD6gYdoqo0yW4fh2jWMCxDYxygSiJSKWzNBttLEX/mWB3/1ipkNlRAver3U6blKETei56Ok3KshhsrrO3tkbGtskv3oGey9C7fI6gsUvkOShxTKZQIFEswkSjPL9AnLXZvb6G0+lSLpQoTk2RqAqNvW3ylSK+rmCnUsRAMAzwcmVS5REUVaPfblMs5fHrO+idOm03JjU9j6ErDJMYy9BQbJvh5tZvpBM11ONE19E/1/PbmJZKr9GkUqqgRhHe7h7Nep1ENxg5fAxdNxgsv4rbaBDGCQQx2UIFLwxQsjlKJ+7Ad4esXbxIpu9Rm5jAGB8nDgN818fK53F8n1BV0G2TeBChVucwbOhYkMQJxUgn2engbu0S2Dr24iRGKY/bHxBGIfl0ms7GGkXbIgl8ktAjDgOIFTpBi0qtDJttor0WjeY+1swoufkpcD16y2tYzSF+OCA2NNS0TcfpUzt0iMz0FIOtHfZXN4m9gLmDB1EqBTzXpT/ok05l0KzUjfeqqrK132TiyGF6XQctNY6uBCiBS29/i7jbxTQN8tNzxKpJoNhoXpsoHeFsb1IpVXBbLXQVQs+ns7+HmctRLpUI6y0a11cJooDc1BiFkSpup0398gopRSXsDkjrBlE+z1CD0twMqZEanZVr9LZ30VAZPXgA1TLoD7uERgZd08mUyiRuSBRD3/WwDx4i8n2SOMQwTPSmR29vF90yCEt50pUynudjaQZKlBCGCdHuGqmihhpHDF0XU4F+Y598pYKegLO1jbdfp0dC+cABMuUa9dfPkPS6JIGPqiZopkZEREhM5cS70Kw0e5cv0e+2GR2pkRsdIVSh3W6jpzNEqkoxVSCKFPwkwU1nMUolbEUh9BzSKZvO1iaGNySKPPTRSYxSFb/ngJUhTiLCduPD+rA3ZZsQ+c5/CJMIW9Hxe0PSuRLECe1r12g1m6SLOUrjY+iqynD5EoHXZzBoY9omRtqm78ekx6epHLmDbmdI69wZ7DigOj2NVqnS63aJooh0vkCoaxi2je+HRKGCMjmB4XtEKZvAcbAtC6+xT9Dr4Ucxo9MzRNkKnXYTzYRs2qBx9QrFbAYtUVG8CDU28d0BjteiVKygDFzaG5t0B30y1RKV+TmioUvjwmVUp0/i9VDQMXN5OnFIaXGB9MQIne0dOitrFFWT/KEZ1HKOTquLpqqoukWs6FjZIgoqrYFDZXaWQTdCS2UwlJAkGDLYWKMfBOi2TW56kRj9xrkzaKDrCm69TiptE3suiucRRT7EMYkCqUwKf2uFnb0GYawzMnOAbLXGcHuP7vp1tMAj6jdJZ0t4WgZfiSgfmCdVKNDd2KS+vkmhXKIwM0WStei3B6QMmygKUVIZFEVDVVRafkh58SCD/oA4ichmsyQ9h8HldTJoUMzCRIlhSsGMYgwnIXRjjLBJmIQoSYKh6wx2d4j9gHw+h4aCt7NLp9umYcbMTU+TyuZxVq7j1RtEvovvDMkUsgSBh5EpYB29F93QqF+8QDjsk85lKc7OkNg2jb09VFPHTlewNBtFVz/heE7RS1l9vVT6gqEq4Puoponb6aI0mliaSTQyQbpQod910EwLtAR/d5NMEoChEEVtksBD8ULCoU+2VIUENs+dRTd0PAUmjx9F6Tq0riwTen0iv49lGWi6xjCK0fNlRg7fQTQY0rl0GS+MGJmdQatWcT2fXqdLOpdDSaXQTIPQ0/AHCursJJHvkcmm8KIIXI+k3cN0XMJYQ58fQzNv/O32kxhdVwg2N0mnbOLAgygg8AM01cMbOhQro8StDo21NYaRT7pYoDg7S9zp0zxzlnTkEyQhWjZNaJq4QGlqiuz0PL3rGzRXrpErFEhPV7HHJ+hsbmOoKmY2h2LaJJoBSUyj06Jw8AShZ0HkYeoxsdMjarboOUOUbIHy1AJukIASosVDlCD4mbCxd0zXNdfQ+ePAd0mcAWHggqGTz5Vx9+vU9/YIfZ+JxQOY2SzOzi7tvX2a9X3Gjx2neOIIvZUVTEVDCRNMFAIvII5DtFSKOIrQk4TGnz6L9s+OHP4/jSyMYZQsDNtEVWIUNUG1dFQtwdAiNEK0JCQVRVhZG8NS8DtNrGIGo1okl4TQ6+Lt72KRYJRy2KUiStpG1xTU0Ec1dbz6PkqnRSptYFgaRhJimmDoCZrbwy5l0GwVLXJAizESH213E63ZwCpm0WIfMwmx0gZ61sbUFeg16W1cR3F9cpUa2cVZCH3Wzp6nXKpg10YxVLDKBVJTEyhRQNBtkS7nMcZr0GtB5OD0mxh6QmayhlXKYSQxuj9EJUJRYwa7W+D2wTZI28aNK+pqgh70iIcdipNjJAQoWoLu9EgpIeHVS+hqjJUyIA5QnA5qzsQo5zCUAHdnjeb2BlroUp2dwJqaIGy3aF67TDpjkRmrEAcOmdEKZqVA5LuEnksqY2IbCildob+/g99qks2mMbNpyKZQ+h1MBXTFgzigu7tDIZPC0BVSpoIWOhiWitLcIWVAdnoehj3UJCBFiO72cdavYRhgaQlat40eeehFGzufRQ19nKuX8NoN8rZOZXIUJZPC3dsk7DTJ5vOkyhV6rTajBw6QKZRo7e2B52HPz5MzUyTtBn53n3DQxRqpkqqVMPNZjGEPXQHFNB6Nup2f6uxsfqo0XiUf+1i2TuL2SKd0hq1dCqUMdjmLFrlYSYiug9LcQ2ntkzY0NCVCS2KSbpvM7DSZfBY6+/RXV4mbTQrlApm5SRQtorN+HTuOyI2MEWkQ+X2Kx4+iRwG9/TqZXBornyGrKwy3N4hbDWzTwCxWsEp5DF1H7XbQQh8tlyHud4h3tyiOFAm9BBQd29KwAo+w2yFbLqOlU2iqThLE6IZJvHkZd2eNcqWIkUTonoOqxdjFLLal4u9v015bxQ5iMsUi6cUFkuGQ1rXrFDN5MqM3JhxWNoM9PYGpgTLoYlbLmPkMxrCH4gwZthpkbBO7UkJPpzCyKdRBH9XWUeMAd38X23Mwi3mSOMQKfXRDxdIh6bbIjJbRCdGJULw+BR2C6ytPaJH78+lC+pfVJCAVBxiV4o3zRezjrV7F3dsmZShUpsbRKyXc5i6D7TWyo2UqYzXMxCc9NoJVKREO+iT4GBNVssUMcadO4g9x2/uka0XSY2W0Sp5o2MKMAnTDQIsjhpubpE0DQ0vI2CbKoI2ZMaDfQgkDcqM1EiVAi30sU0Gp7+Bvb3zYTlkftSzjw4rTQ1d8rHKelKGhJQGD5fMM6rukLZ3yzBR6Lk1nZZmw1yI3MkK+UiH2fPJT05jpNP1WC0sBs1ymmE6jtvZwOg3UwMEoFdAtDSVlogx6mCpYhorTaBJ0e2TLGTTtxudwdEVBUxWGjV0KIwWs8RJet0E29klZGsr+Fn59m3TGQrUU1GEHzQC9mCOT0VE7bbpXVkh6XdLlAvmZcTQtobt2HTOIyJbLEA2JYp/SwXlyWYu15QsUSgXsaoW8rhG1mnR2tzBMhVQ5T7qYwzAN9NBD00BVYbi3g9lpkhutYXoehhpg6CEpxcNt71GdGkXN2Gieg24oqFpAuHWNpNMklbYwiNG8AUbGIjtSIZUy8TbX2buyjK0r5CZr5CZqxIHDcHudQtYmN1rFcXqk8xmyM1PgDenvbpEtF7GLeYzhgP7OFpo3xCzmsPIZ7LEKitMn6TXRTB3NUEla+yiDNnYhixr7KEqMYZroSkA87JCfGkH3+pi2ShQO0b0+wfoKetDHSmk3vo4gdECHTCmPmoT4q1fx6jskgcPo0gHM0Rru6gpRt0WhViQ3ViVyumRGS2QnRlC8PhoK+tgo2ZSB1mvS398m6LbIVIqks2nssREsd4AWBmi2QeIP6KyuUMwYaJaKqYQogzapcg6z10Q3FaxSHq3fRbUNDC1Ga+zgbl3/uJ23H7KM5CE98TEI0MpFDDUBNaZz8Rxuv0WhkKYyN4k2UqB16SKx51OaGsfOZ7EMjezMLKlsHq8/xFQNUqUqRTtB6zcZthsYcYA9MYJdLqClDPROE5UEM20TtOsEjT00SyOjg60r6J6DaWjE9T3yxQJGOk00GJJRYjKaQrRymaTTIpfPobkDND0m0WLM0SIpPYFul+6VZYJ+h2K1SGFuEmKf+ltvYmka5ekaWirCyKXJTk5gENPZ3SFfKpIaGSHleVitFu32PigJudERTMvCSFuYnovqDtFU6GxvYIVDUvkKuqKh+gM0y8AMPZx2i/HZWXTlxiLADiM0w8e/fIEocEinDDQF4m7zxvk4bWIYEG6t0V1fRQsjypUqmfFxGPRxtzcxUyb5fIrY6ZEvlcjUaoShQ7+xS25mAss2sJOY4e42veYuhXKeVMZGz6XBG2Ab6o3fcdqife0SJV0hVcxhEhIGDqmUiTocEA+6pOYmUPUQkwBVA2Po4G+sY+kBlq1jaAne5hqZyTFShQxq6OFsrNLdWce0VMrzi6QLRYJ6g7DXI1sskqlWGHZaFBcXyFTKDNptoigkXSuRzdr4u1v47caNYy3kyeQzpGslwlYTVQFdix6N/cFDtPc/ahsKlq2hKhEGISlDQet2sUwDM5NCIcbUQEsCtPouvbUrpC0Nw1DRY5cwCEiVK5i5PAwGtJaXIY5Qk4SJw0uouoZzfRUljihPjGFnUoRDh9LsDJl8Aac3IJUroBXzpIyEuNNk2GqieANStQqZch7dUNAHPQw1wcxmiFot4tYOxUqOIHAwCEnrCnbKQm3VMfMZzLSGptyYO6aUEG3tKmGvSSqfQtdj9MDBtA2MagXbtInaPfpra3iOS2linOLMDJpusvX6GcqlCrmpMQxNxSgVyI+PY+k6/XaHdDqDVSiR0XTiYR93sI9FTKpWwSgXUFVQWg2UKECzTcL9PeJBH72QI5246GqIrgWYWkjodCmPVEGN0Q0Fze1gR0PCjetPDAfdny+MVX5RdfoYoY9RyGLVylhAUt+ntXYdLfQYnZrAKOYI+h3qly6QLeaYnJ0ipcYQu6ixh6WBToCqxOimiqHGaIaCrsWovsvg9esoz33wp5PsuEGcNzH5HxOqWLuxRSwIQNNB1wlUhUzPIaUbKLpKp9NCzaQx0yki16e7u4dtGKiKhlYpo6gK6VyB1t4udjpFout4fYcoCrHzOdBVXNNHTUDXdILeANO2iZMEVIhVhUhX6O+2KHlpUqUcrjsAEqxM+saiqN9j2GpBGGKGOtl8FSejM4g8oqFD0UiRCkGJIoZqhJsxMYiJPA9V00il0sSuR6vRIFJAS9sYhRx6JkUu1qlvbpLNFwiThH63j2YYpIoVNF0njGMMRcOMXIaug57NESUJUQKqqhJ4LnGnQ+p/3C7zDZ2g36M2OcGw3yd2hnTrdUxdg9CnMjFOrNu0Wn1wBhSLOZLIJwp8EhSMdI4giOgOXKqVPFp8Y2+i0xvg+AGFYgU9lcLOZkmCCKc//H+0cCZLliU9EXYNEXGGe3Oo6uquv8GMHU/KnjXvxY4FZhg/BjVk5Z3OiRMRklhkv4Mkk7vb55AcCCbctwNIE/LpCR4B8oGijLHfwAi0kiHEcASUgFbrR4KREhiGKSW0bpg+/Q33X7+QRgD7ATsO2DC8/u0rjtFxub7j6XwGPOBEOLaK5XSCg3A8KswD89Mzhjlw/wWNhtu24fT77xjEWD5/Qrve0PYDMq/IYPzv/r/g1xNmUXAAYYF5mvF2ecd6WqEi6GYolNFbR398gJp5KhAWhDmKMniZcdw37I8GvlfwvWI+nTH98QkXr3hsd5w14YUmeK3osaNKxml9xuVyA+WC+bSi7Xcc7z+RydC0QM4vmNYVuRS8f//24eTOKx6XC/pxYH55gZxf8HADW8UyJWy/fmI+rRgW4DKjtoA5I13eEL3j9PqCY9+gxJifVrTW4WPg2Dfc396wNMXp6xeMpxm17jgeD7xOJzAE7I7jqKC5IJNhu92xvjzDidD3HY/7DRaBZV0/5uV8gjHh+u0H8rpCU0Hfdhz1wOm334EgcErwbceUFI/9AV0n7ONASgpm+oiX9x3zNANaMBBI7kjzBDKD7zvev3/DSRW1V3z+4yu2csLldgUdG56fT6B+QK1jG458eoEdjtoP8NOCJIrrt+9QEMYYmJ+egKyY//iCy+UX7PrAKxeYJvz49obXP/6EMaOsK0Z9oCjQjgeYFLnMuLeK07pgGwO391/IAagmiAETC7ww0nnFuFxh24b950+UnOBgfP7zH/HYdtyuV1BhnM7PiL1htIFlWjGI0dqBzgyaZ2hS6M+/4/G4gyRhff0EaEL5i4sKIgQn0Gjo9Qr/dMZYMlQEug8sLvDjQCwFF+lYaIFuBCjh8v0b1nkGlBAUH5BjVuRc8L7d0B8bnhrQfl2Rn59ALycc3rHdbvg9n7BywaNeYeSIkiAl43q7gVPBejrB9452ucH6QJ8Cy9Mz5tMTwIL9csHRDKfnFxz7gXp/YP7jN+ypwUdHEQKjY7td8PT6iv1waFnxGI4Vivb3H5hOKzQnRAS8HyhPZyACj/d3jOOAbRVujqevX5DmjMdx4P7rDX/+8QU+DPVyQyZGmc4Id/zab0jrglJm1O9vSP2DQePnE/S8AqcZEo7t/YJpOmEMx3G/owfw8vUreh8gVZDd0Zlg1kFLBuBoR8OSEmzb4dsOcsW6/oZaNyxzRjqtqHXH2DbsP34gM8GOA8uf/4CmBeN+xwTCVCYM7/BjRyRBZEF4hzeB5AWlJFz/5+9QJuxHxfz6YXbkT5+wXy6wyx2npxfcjwNmHS6K6eUZbIaIgEYgRgdLoA9Hmk5oYAQxHtcr1DpIgHMStDB4SShlxrg9QEfH/e0duWQcveH161cMJVz+7yfWvCIXRdiAWMdAQKYVZoTqAj2/4owr7v/znwAJhgH56RnT+QydZ1y/fYeRQKcZcWwfzvK8QEoGRCAkf83VFaUsECI4MZIwvHX47QLEQHp5xoDjbh3T5xc4M+TbN8TesH3/gZIKptcn9CmjJMXP//pv/ONvf8MuAxfaUDqh0IRJCn79uoBOC6apgPYd9HbBlQO+zDifzyBN4KTYb3cwMyhl9FoRZujPr/D0IcLG8fi4jY8H5tMZrRlkPmN0hzwqfH8gKFCmCcKAjYbl+Rm9VbS6o/76iVN8pIKvv/8Jygvu9zt6rXh5eQZHx7i9I2lByiuqBN6uvzCfTjitJ4zHjnq94toOzF9eIaJYnl9w7A/YMRAAZFqwv1/hLFifX8DBqMsAS0BuDVo70tOKOxuqACoCuWyQywYrJ5R5Rj8qZmGU8xn1dkXUiv12QW8HIIrln/4ZwoT95xukdazz8mFyXC8ozydYGMgDt7cLXn//AmsH+v0Gt44RgfXTZzQV6PkMexwgB0gFPgb2fYMp4/TpFeSBYQbVBL+/gcVR1mc8uiNYgHD40TAeN0xFIAhUXTEtT2AStPcrxBz79YbWGqZlwfzbJ/TWsF9uOJ9WuPcP0bbdUU4ncC64XB/AfMI8Zcz7G359/wF3gFLG9PSMcj4jzzPev/3AcGB5mtHuP1FrxfL7bwhVRADeDUULfKuYy4QeCTWApIK2b2AfOOqO08sTwsZH8cS6YqMMPwb6+w3YDxAR+Lxifjqj3x/olxteTs8YVEHhqOFALhARbPuOZT5BOEE6cHv7gWo/UZYFy/kZuqyQIGzvV5AqIAn9saNJwD7PWEoBWUMcd+Rlwu1yQTl/RmsGnVfwZqB94PLzB15eP0NLwrE/kIhw/vIF99sVfjTE9kD0AxGB59++wBB4bBva44PjYWaMMbArQYRBAXAA5AFlgfWBiACYQObAv7+D/uPf/vVfpG5Z+sjkQQgxhg5yfAgKYQcYwQh2Z29NmKA8ZevDGo+xkKYmOTeqbWYS2yMaEyQiwMTEBLawIJZBZiUCBjLt8zYxIDbMBEREDI/oEjG7cA+4kJcu/fka0dWFOgDA+0TCFj6URIcIURqcRsdogtYS1bPq+diaiYFTStRjxMHec9GgvT4F8w6QcUSG6GCKZPAY7hYILcPLELmJ2eKSdrCYhs8NfGeLPAgtISaG5UF8BCHCg0N0p2FZEJJJ1CN6H4Na1ppyDq77qQNHARaoHgwXh0Pc1Y3NXQMlbYnGuh8P0Smbm4MHE1Py0LLFcXlBwggLEc1DWDCamUcYIjjpDFjPgU6RpMaAWi5XtLE4cycyQYwkBAHD4dsphMYYAypMTEQiLObRwkw4PA8LJ32+GLhqtzWxMDE7mK3XI8UyveOoZ9YUI2KI2dy0vKO1lHMBBQmB4O4AJJR6dnQXEunwzuZlBDXVRMF89KNTSQUm7y+GQ4K4M4g8AkzgEehEoe5GEOmpQwNkFBDOaWCYGmtFOCWiYmNQaH5En0dmRQKn42juQsPFGSrGNlSMA6MnmZmG0zGchxBnZg5rTU31tigvYYf0YfCy3rjuT666x7AkJQ1vRxJJzkx87HvwWnxEU4Iz+8hMLhERwdwRwqbpgUGsUToBQGvFU6ocrgRXJ2oUIRxRUHLlHYmEqCtqUAiYB7qLBmEcx+RzuZKbSMrd97roPDWvdQKzOzCYmYnJMUYiFbPRsqnsTOo0RoGkJkDqww2cOrd+CqHjrz0mSeyHdVeKQuTMABAAfEjIdBzd+1Sy9N4gJCFwgVnWpAYf6mZy6HJhZQ4fNFHMrdeglHsMY4EStBwDMPe9ECiEBTxskpR6Da/idmo+Ok3JcJiUTnASj1QaOeXhYQzPA+PIiblHpwLW3s2iaEU9TjRNOwjQYSdnecQwluHJs/ZAEIUTU5AQS4RjmAU5U6RpQ4C9BLF7aiP2Ipl9PwpSqszC7sFB5ERBOeoSnJrbQLhTSGroriE8CODoltKcDHGfG7kPmFBKTZqVIHkk97kz2SFd1Jdt7hPcTUy5aozVRDYfXVkgFK4Q7sMASK5rpJnbmCvFxWIoT3kEgHR4xuHJpjhCEMNGUCClnNEjKgex90FFVIUFAy3VPqoS0nD0xCoQbRIkiFCw9jGq2vKYg6XS/njWkitiZB8mRnKIJIxhUD49eEyV3ScwmUcQMcXojYQgITLARCWoRNDojMrWT131rky5u40cMY1AgztSZIa5YC3V2pHhiEKiEQR3d0nMm/ca3IqoAE5GI9icmoqIiuJo3URERnRw7iWsp+AgJ3cw2N2tiLLvjzXnjBHzJY71LhRTZ1Rz84lp7mEjC0/u5plZu3MflCrDJQUn82FOHibcFTF3tg6EFs/cDq8ioVkpkVsO4bHXOmRemjUTzZPTUTMCiJyqEEon3q23nJKi9W6FRB0f7BqAAKewEQEWI2FS9zxGjTRP3VudXAUW0d3C1CKnlF1EMawTEUVrjXiam5JoMzvQW07KGoATqREpj2AbzpHxKExNmJWtu4fogdanYNmFKA/RR7SeVT5aUV30iNEmZ+kSLA4xJiEE+zALzu5sodYHVpLFwM29sQvQS7pCKRkMqR4zWGsOUmamOroZjFmFmbSnY0yeKWoZEl1uxVSieqap1KBw8j4501HcztBSm8XgcDWgx19PCIPIzZgBCeLei3lEK+JjCaHKZlkQ6XBvMq271TZRmnZtS1NEcaIjbGRK0sy6cFhyACQ8iJm0t2ycD3ca1kamUg4RYowhTjEUYyEPMsoXGsenOC+3MTpStzkghyalYUNH5kbtmMI9kPOujimcwokOAamDO9pInKe6y/sqahPv4crqHo6AlTHpxh5JOiJTimZlRxCMYigom3snciEVE8REKj16L52WO7MTBROGCVOwu6Gr3DTGmYTDQE1aJPggUKAop4BLEA0fJg1Sg6VrRBYR81pXmqcH3IqrHGPfc5omb72PIpqMd/GxTVNaYj9G55TCASO3rKU0O7aJYAR+2Tyk0+jKnEf0nnLK1JkqR2irVWWeKlPSMbpzGBelfHj/qymMCdPysDYSwlhQRUU0BasBwxweBgs3Yc3hEQOxLZirsMcUATKzIM3diTt3zyHpIcNT+NyNdCd3JZWmHMsIt3ATMDk4wB7qlJtwRjQLJU4DaAPeiUIZJClYRzcnbqknuXNEUk3Uhg3QX89eD5VgISFjqdmJG/pIw2ykNDnAgFkKkUpG4tnR03WCOXO4gJx4tMVFGkh7cBree0r8fJMm4QQXomTDyAFLquIRZmaQXCK8h4blIB6BQLgpMcFVugKlhR/krgCEkrRoPZGmgeECREB0hBkBIBB5cOH/B3c5NWpyQ0oyAAAAAElFTkSuQmCC) no-repeat;          background-size:100% 100%;          background-position: 0px 0px;          }          .dongcuoi tr td:first-child,.dongcuoi tr th:first-child{          border-left: 1px solid  #cc0000;          }          .dongcuoi tr td:last-child,.dongcuoi tr th:last-child{          border-right:1px solid  #cc0000;          }          .VATTEMP .statistics table {          background-position: bottom;          border: 0 none;          font-size: 15px;          margin: 0;          position: relative;          z-index: 2;          width: 100%;          table-layout: fixed          }          table {          border-collapse: collapse;          font-family: 'Time new roman';          font-size: 15px          }          .VATTEMP .statistics table th.h1 {          font-size: 16px;          text-transform: none;          color: #cc3333 !important;          color: #000;          padding: 2px;          font-weight: bold;          border-right: 1px solid #cc3333;          border-top: 1px solid #cc3333;          background-color: rgba(255, 255, 255, 0);          }          .VATTEMP .statistics table th.h2 {          font-size: 14px;          text-transform: none;          color: #cc3333;          font-weight: normal;          border-bottom:  #cc3333 solid thin;          border-right: 1px solid #cc3333;          border-left: 1px solid #cc3333;          border-top: 1px solid #cc3333;          background-color: rgba(255, 255, 255, 0);          }          .VATTEMP .statistics table td.stt {          text-align: center          }          .VATTEMP .statistics table td.stt2 {          text-align: center;          color: #cc3333          }          .VATTEMP .statistics table .back td {          color: #cc3333;          font-family: 'Time new roman';          font-size: 16px          }          .VATTEMP .statistics table .noline td {          border-bottom: none;          border-right: 1px solid #cc3333;          border-left: 1px solid #cc3333;          border-top: none          }          .VATTEMP .statistics table .noline td:first-child {          }          tfoot tr td p, tfoot tr td b{          font-size: 16px}          tfoot tr td   .clsCol p{          margin-bottom:2px          }          .VATTEMP .statistics table td {          border-bottom: none;          border-right: none;          border-left: none;
                         <!--   padding: 2px;-->          word-wrap: break-word;          overflow-wrap: break-word          }          .VATTEMP .statistics tr td.back-bold {          font-size: 16px;          border-bottom: none;          color: #000          }          .VATTEMP .statistics table .back-bold {          padding-right: 5px;          text-align: right          }          .VATTEMP .statistics tr td.back-bold2 {          font-size: 16px;          border-bottom: none;          color: #000          }          p.input-txt {          color: #000;          line-height: 16px;          border-bottom: 1px dotted rgba(0, 0, 0, 0.5);          font-family: 'Time new roman';          }          .clsCol p {          padding-top: 3px;          font-size: 16px;          line-height: 16px;          margin-bottom: 1px;          }          .col-title {          width: 1%;          white-space: nowrap;          }          .col-title p{          color: #cc3333 !important;          }          .clsCol {          display: table-cell;          }          .clsTable {          width: 100%;          }          .clsTable {          clear: both;          }          .clsCol {          display: table-cell;          }          p {          margin: 0px 0 0px 0;          }          .VATTEMP .statistics table .back-bold2 {          padding-left: 5px;          text-align: left          }          .VATTEMP .statistics tr.bg-pink td {          font-size: 15px;          text-align: right;          color: #000;          background: none repeat scroll 0 0 #fedccc          }          .VATTEMP .payment,          .date {          margin: 0px 0;          text-align: center;          width: 35%          }          .VATTEMP .payment {          float: left          }          .VATTEMP .payment p,          .date p {          margin: 0          }          .VATTEMP .date {          float: right;          height: 120px          }          .VATTEMP .input-date {          width: 40px          }          .VATTEMP .input-name,          .back-bold,          .input-date {          color: #000;          font-family: 'Time new roman';          font-size: 15px          }          .footer_invoice .clsCol p{          font-size: 15px          }          .fl-l {          float: left;          font-family: 'Time new roman';          font-size: 15px          }          .bgimg{ border: 1px solid #000000; cursor: pointer;width: 170px;} .bgimg p { color: #000000;padding-left: 13px;text-align: left;}          p {          font-family: 'Time new roman';          font-size: 15px          }          .VATTEMP .header .number {          color: #333;          font-family: "Times New Roman" ,Times,serif;          }          .item {          color: #000          }
                    </style>                    
                    <script>          function displayCert(serialCert) {          plugin().ShowCertInfo(serialCert);          }        </script>
               </head>
               <body>
                    <div id="printView" style="background-color: rgba(255, 255, 255, 0);width: 750px;&#xD;&#xA;    margin: auto;">
                         <div id="SolutionVNPT" style="margin-bottom:0px;margin-top:3px;font-family: 'Time New Roman';font-size:9px;border-bottom:0px dashed #074a8e;text-align: center;background-color: rgba(255, 255, 255, 0);width: 769px; margin:auto">
                              <label style="font-family: 'Time New Roman';font-size: 13px;    color: #cc3333;">              Đơn vị cung cấp giải pháp hóa đơn điện tử: Tổng công ty dịch vụ viễn thông - VNPT Vinaphone, MST:0106869738, Điện thoại:18001260            </label>
                         </div>
                         <xsl:for-each select="HDon//DLHDon">
                              <div class="VATTEMP" style="background-color: rgba(255, 255, 255, 0);">
                                   <div id="quantitypages" style="padding-left:0px;    border-bottom: #cc3333 solid thin;">
                                        <xsl:call-template name="main">
                                             <xsl:with-param name="pagesNeededfnc" select="$pagesNeeded" />
                                             <xsl:with-param name="itemCountfnc" select="count(NDHDon//DSHHDVu//HHDVu)" />
                                             <xsl:with-param name="itemNeeded" select="$itemsPerPage" />
                                        </xsl:call-template>
                                   </div>
                                   <!--end header-->
                                   <!--dialog server-->
                                   <div id="dialogServer" style="background-color:white;display:none">
                                        <xsl:variable name="sc">
                                             <xsl:value-of select="//*[contains(@Id,'serSig')]/ds:KeyInfo/ds:X509Data/ds:X509Certificate" />
                                        </xsl:variable>
                                        <div style="color:blue" id="messSer">Unknown!</div>
                                        <br />
                                        <br />
                                        <a href="#" onclick="displayCert('{$sc}')">
                                             <div style="color:#184D4E">Xem thông tin chứng thư</div>
                                        </a>
                                   </div>
                                   <!---->
                                   <!--dialog client-->
                                   <div id="dialogClient" style="background-color:white;display:none">
                                        <xsl:variable name="sc2">
                                             <xsl:value-of select="//*[contains(@Id,'cltSig')]/ds:KeyInfo/ds:X509Data/ds:X509Certificate" />
                                        </xsl:variable>
                                        <div style="color:blue" id="messClt">Unknown!</div>
                                        <br />
                                        <br />
                                        <a href="#" onclick="displayCert('{$sc2}')">
                                             <div style="color:#184D4E">Xem thông tin chứng thư</div>
                                        </a>
                                   </div>
                                   <!---->
                              </div>
                         </xsl:for-each>
                         <div class="clearfix" id="bt" />
                    </div>
               </body>
          </html>
     </xsl:template>
</xsl:stylesheet>
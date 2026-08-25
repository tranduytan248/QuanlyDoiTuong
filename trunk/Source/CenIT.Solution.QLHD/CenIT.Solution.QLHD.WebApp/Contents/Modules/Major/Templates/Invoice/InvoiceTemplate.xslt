<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
     <xsl:output method="xml" indent="yes" />
     <xsl:template match="/Invoices">
          <DSHDon>
               <xsl:apply-templates select="Inv" />
          </DSHDon>
     </xsl:template>
     <xsl:template match="Inv">
          <HDon>
               <xsl:if test="key">
                    <xsl:if test="key != ''">
                         <key>
                              <xsl:value-of select="key" />
                         </key>
                    </xsl:if>
                    <xsl:if test="key = ''">
                         <key />
                    </xsl:if>
               </xsl:if>
               <xsl:apply-templates select="Invoice" />
          </HDon>
     </xsl:template>
     <xsl:template match="Invoice">
          <DLHDon>
               <TTChung>
                    <SHDon>0000000</SHDon>
                    <xsl:if test="Pattern">
                         <xsl:if test="Pattern != ''">
                              <KHMSHDon>
                                   <xsl:value-of select="Pattern" />
                              </KHMSHDon>
                         </xsl:if>
                         <xsl:if test="Pattern = ''">
                              <KHMSHDon />
                         </xsl:if>
                    </xsl:if>
                    <xsl:if test="Serial">
                         <xsl:if test="Serial != ''">
                              <KHHDon>
                                   <xsl:value-of select="Serial" />
                              </KHHDon>
                         </xsl:if>
                         <xsl:if test="Serial = ''">
                              <KHHDon />
                         </xsl:if>
                    </xsl:if>
                    <xsl:if test="PaymentMethod">
                         <xsl:if test="PaymentMethod != ''">
                              <HTTToan>
                                   <xsl:value-of select="PaymentMethod" />
                              </HTTToan>
                         </xsl:if>
                         <xsl:if test="PaymentMethod = ''">
                              <HTTToan />
                         </xsl:if>
                    </xsl:if>
                    <xsl:if test="CurrencyUnit">
                         <xsl:if test="CurrencyUnit != ''">
                              <DVTTe>
                                   <xsl:value-of select="CurrencyUnit" />
                              </DVTTe>
                         </xsl:if>
                         <xsl:if test="CurrencyUnit = ''">
                              <DVTTe>VND</DVTTe>
                         </xsl:if>
                    </xsl:if>
               </TTChung>

               <NDHDon>
                    <NBan>
                         <Ten>VĂN PHÒNG ĐĂNG KÝ ĐẤT ĐAI KHÁNH HOÀ</Ten>
                         <MST>4202039053</MST>
                         <DChi>01, Lê Lợi(Phường Xương Huân), Phường Nha Trang, Tỉnh Khánh Hòa</DChi>
                         <SDThoai>02583820663</SDThoai>
                         <STKNHang></STKNHang>
                         <TNHang></TNHang>
                    </NBan>

                    <NMua>
                         <xsl:if test="CusCode">
                              <xsl:if test="CusCode != ''">
                                   <MKHang>
                                        <xsl:value-of select="CusCode" />
                                   </MKHang>
                              </xsl:if>
                              <xsl:if test="CusCode = ''">
                                   <MKHang />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="CusBankNo">
                              <xsl:if test="CusBankNo != ''">
                                   <STKNHang>
                                        <xsl:value-of select="CusBankNo" />
                                   </STKNHang>
                              </xsl:if>
                              <xsl:if test="CusBankNo = ''">
                                   <STKNHang />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="CusBankName">
                              <xsl:if test="CusBankName != ''">
                                   <TNHang>
                                        <xsl:value-of select="CusBankName" />
                                   </TNHang>
                              </xsl:if>
                              <xsl:if test="CusBankName = ''">
                                   <TNHang />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="Buyer">
                              <xsl:if test="Buyer != ''">
                                   <HVTNMHang>
                                        <xsl:value-of select="Buyer" />
                                   </HVTNMHang>
                              </xsl:if>
                              <xsl:if test="Buyer = ''">
                                   <HVTNMHang />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="CusName">
                              <xsl:if test="CusName != ''">
                                   <Ten>
                                        <xsl:value-of select="CusName" />
                                   </Ten>
                              </xsl:if>
                              <xsl:if test="CusName = ''">
                                   <Ten />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="CusAddress">
                              <xsl:if test="CusAddress != ''">
                                   <DChi>
                                        <xsl:value-of select="CusAddress" />
                                   </DChi>
                              </xsl:if>
                              <xsl:if test="CusAddress = ''">
                                   <DChi />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="CusPhone">
                              <xsl:if test="CusPhone != ''">
                                   <SDThoai>
                                        <xsl:value-of select="CusPhone" />
                                   </SDThoai>
                              </xsl:if>
                              <xsl:if test="CusPhone = ''">
                                   <SDThoai />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="CusTaxCode">
                              <xsl:if test="CusTaxCode != ''">
                                   <MST>
                                        <xsl:value-of select="CusTaxCode" />
                                   </MST>
                              </xsl:if>
                              <xsl:if test="CusTaxCode = ''">
                                   <MST />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="CCCDan">
                              <xsl:if test="CCCDan != ''">
                                   <CCCDan>
                                        <xsl:value-of select="CCCDan" />
                                   </CCCDan>
                              </xsl:if>
                              <xsl:if test="CCCDan = ''">
                                   <CCCDan />
                              </xsl:if>
                         </xsl:if>
                    </NMua>

                    <xsl:apply-templates select="Products" />

                    <TToan>
                         <xsl:if test="Total">
                              <xsl:if test="Total != ''">
                                   <TgTCThue>
                                        <xsl:value-of select="Total" />
                                   </TgTCThue>
                              </xsl:if>
                              <xsl:if test="Total = ''">
                                   <TgTCThue />
                              </xsl:if>
                         </xsl:if>
                         <!--VAT_Amount-->
                         <xsl:if test="VATAmount">
                              <xsl:if test="VATAmount != ''">
                                   <TgTThue>
                                        <xsl:value-of select="VATAmount" />
                                   </TgTThue>
                              </xsl:if>
                              <xsl:if test="VATAmount = ''">
                                   <TgTThue />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="DiscountAmount">
                              <xsl:if test="DiscountAmount != ''">
                                   <TTCKTMai>
                                        <xsl:value-of select="DiscountAmount" />
                                   </TTCKTMai>
                              </xsl:if>
                              <xsl:if test="DiscountAmount = ''">
                                   <TTCKTMai />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="Amount">
                              <xsl:if test="Amount != ''">
                                   <TgTTTBSo>
                                        <xsl:value-of select="Amount" />
                                   </TgTTTBSo>
                              </xsl:if>
                              <xsl:if test="Amount = ''">
                                   <TgTTTBSo />
                              </xsl:if>
                         </xsl:if>
                         <!--Amount_words-->
                         <xsl:if test="AmountInWords">
                              <xsl:if test="AmountInWords != ''">
                                   <TgTTTBChu>
                                        <xsl:value-of select="AmountInWords" />
                                   </TgTTTBChu>
                              </xsl:if>
                              <xsl:if test="AmountInWords = ''">
                                   <TgTTTBChu />
                              </xsl:if>
                         </xsl:if>
                         <THTTLTSuat>
                              <LTSuat>
                                   <!--VAT_Rate-->
                                   <xsl:if test="VATRate">
                                        <xsl:if test="VATRate != ''">
                                             <TSuat>
                                                  <xsl:value-of select="VATRate" />
                                             </TSuat>
                                        </xsl:if>
                                        <xsl:if test="VATRate = ''">
                                             <TSuat />
                                        </xsl:if>
                                   </xsl:if>
                              </LTSuat>
                         </THTTLTSuat>
                         <TTKhac>
                              <!--Extra9-->
                              <xsl:if test="Extra9">
                                   <TTin>
                                        <TTruong>Extra9</TTruong>
                                        <KDLieu>string</KDLieu>
                                        <xsl:if test="Extra9 != ''">
                                             <DLieu>
                                                  <xsl:value-of select="Extra9" />
                                             </DLieu>
                                        </xsl:if>
                                        <xsl:if test="Extra9 = ''">
                                             <DLieu />
                                        </xsl:if>
                                   </TTin>
                              </xsl:if>
                              <!--Extra10-->
                              <xsl:if test="Extra10">
                                   <TTin>
                                        <TTruong>Extra10</TTruong>
                                        <KDLieu>string</KDLieu>
                                        <xsl:if test="Extra10 != ''">
                                             <DLieu>
                                                  <xsl:value-of select="Extra10" />
                                             </DLieu>
                                        </xsl:if>
                                        <xsl:if test="Extra10 = ''">
                                             <DLieu />
                                        </xsl:if>
                                   </TTin>
                              </xsl:if>
                         </TTKhac>
                    </TToan>
               </NDHDon>
          </DLHDon>
     </xsl:template>
     <xsl:template match="Products">
          <DSHHDVu>
               <xsl:for-each select="Product">
                    <HHDVu>
                         <xsl:if test="ProdName">
                              <xsl:if test="ProdName != ''">
                                   <THHDVu>
                                        <xsl:value-of select="ProdName" />
                                   </THHDVu>
                              </xsl:if>
                              <xsl:if test="ProdName = ''">
                                   <THHDVu />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="ProdUnit">
                              <xsl:if test="ProdUnit != ''">
                                   <DVTinh>
                                        <xsl:value-of select="ProdUnit" />
                                   </DVTinh>
                              </xsl:if>
                              <xsl:if test="ProdUnit = ''">
                                   <DVTinh />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="ProdQuantity">
                              <xsl:if test="ProdQuantity != ''">
                                   <SLuong>
                                        <xsl:value-of select="ProdQuantity" />
                                   </SLuong>
                              </xsl:if>
                              <xsl:if test="ProdQuantity = ''">
                                   <SLuong />
                              </xsl:if>
                         </xsl:if>
                         <xsl:if test="ProdPrice">
                              <xsl:if test="ProdPrice != ''">
                                   <DGia>
                                        <xsl:value-of select="ProdPrice" />
                                   </DGia>
                              </xsl:if>
                              <xsl:if test="ProdPrice = ''">
                                   <DGia />
                              </xsl:if>
                         </xsl:if>
                         <!--Amount-->
                         <!--<xsl:if test="Amount"><xsl:if test="Amount != ''"><Amount><xsl:value-of select="Amount" /></Amount></xsl:if><xsl:if test="Amount = ''"><Amount /></xsl:if></xsl:if>-->
                         <!--VATRate-->
                         <xsl:if test="VATRate">
                              <xsl:if test="VATRate != ''">
                                   <TSuat>
                                        <xsl:value-of select="VATRate" />
                                   </TSuat>
                              </xsl:if>
                              <xsl:if test="VATRate = ''">
                                   <TSuat />
                              </xsl:if>
                         </xsl:if>
                         <!--VATAmount-->
                         <xsl:if test="VATAmount">
                              <xsl:if test="VATAmount != ''">
                                   <TThue>
                                        <xsl:value-of select="VATAmount" />
                                   </TThue>
                              </xsl:if>
                              <xsl:if test="VATAmount = ''">
                                   <TThue />
                              </xsl:if>
                         </xsl:if>
                         <!--Total-->
                         <xsl:if test="Total">
                              <xsl:if test="Total != ''">
                                   <ThTien>
                                        <xsl:value-of select="Total" />
                                   </ThTien>
                              </xsl:if>
                              <xsl:if test="Total = ''">
                                   <ThTien />
                              </xsl:if>
                         </xsl:if>
                         <!--IsSum-->
                         <xsl:if test="IsSum">
                              <xsl:if test="IsSum != ''">
                                   <TChat>
                                        <xsl:value-of select="IsSum" />
                                   </TChat>
                              </xsl:if>
                              <xsl:if test="IsSum = ''">
                                   <TChat>0</TChat>
                              </xsl:if>
                         </xsl:if>
                    </HHDVu>
               </xsl:for-each>
          </DSHHDVu>
     </xsl:template>
</xsl:stylesheet>
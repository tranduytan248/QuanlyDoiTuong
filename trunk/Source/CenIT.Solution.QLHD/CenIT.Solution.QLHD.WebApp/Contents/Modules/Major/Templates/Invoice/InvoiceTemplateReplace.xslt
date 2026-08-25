<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />

  <xsl:template match="/ThayTheHD">
    <ThayTheHD>
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

      <xsl:if test="InvoiceNo">
        <xsl:if test="InvoiceNo != ''">
          <InvoiceNo>
            <xsl:value-of select="InvoiceNo" />
          </InvoiceNo>
        </xsl:if>
        <xsl:if test="InvoiceNo = ''">
          <InvoiceNo />
        </xsl:if>
      </xsl:if>

      <xsl:apply-templates select="TTChung" />
      <xsl:apply-templates select="NDHDon" />

    </ThayTheHD>
  </xsl:template>

  <xsl:template match="TTChung">
    <TTChung>

      <xsl:if test="MHSo">
        <xsl:if test="MHSo != ''">
          <MHSo>
            <xsl:value-of select="MHSo" />
          </MHSo>
        </xsl:if>
        <xsl:if test="MHSo = ''">
          <MHSo />
        </xsl:if>
      </xsl:if>

      <xsl:if test="SBKe">
        <xsl:if test="SBKe != ''">
          <SBKe>
            <xsl:value-of select="SBKe" />
          </SBKe>
        </xsl:if>
        <xsl:if test="SBKe = ''">
          <SBKe />
        </xsl:if>
      </xsl:if>

      <xsl:if test="NBKe">
        <xsl:if test="NBKe != ''">
          <NBKe>
            <xsl:value-of select="NBKe" />
          </NBKe>
        </xsl:if>
        <xsl:if test="NBKe = ''">
          <NBKe />
        </xsl:if>
      </xsl:if>

      <xsl:if test="DVTTe">
        <xsl:if test="DVTTe != ''">
          <DVTTe>
            <xsl:value-of select="DVTTe" />
          </DVTTe>
        </xsl:if>
        <xsl:if test="DVTTe = ''">
          <DVTTe />
        </xsl:if>
      </xsl:if>

      <xsl:if test="TGia">
        <xsl:if test="TGia != ''">
          <TGia>
            <xsl:value-of select="TGia" />
          </TGia>
        </xsl:if>
        <xsl:if test="TGia = ''">
          <TGia />
        </xsl:if>
      </xsl:if>

      <xsl:if test="HTTToan">
        <xsl:if test="HTTToan != ''">
          <HTTToan>
            <xsl:value-of select="HTTToan" />
          </HTTToan>
        </xsl:if>
        <xsl:if test="HTTToan = ''">
          <HTTToan />
        </xsl:if>
      </xsl:if>

    </TTChung>
  </xsl:template>

  <xsl:template match="NDHDon">
    <NDHDon>

      <xsl:apply-templates select="NBan" />
      <xsl:apply-templates select="NMua" />
      <xsl:apply-templates select="DSHHDVu" />
      <xsl:apply-templates select="TToan" />

    </NDHDon>
  </xsl:template>

  <xsl:template match="NBan">
    <NBan>

      <xsl:if test="Ten">
        <xsl:if test="Ten != ''">
          <Ten>
            <xsl:value-of select="Ten" />
          </Ten>
        </xsl:if>
        <xsl:if test="Ten = ''">
          <Ten />
        </xsl:if>
      </xsl:if>

      <xsl:if test="MST">
        <xsl:if test="MST != ''">
          <MST>
            <xsl:value-of select="MST" />
          </MST>
        </xsl:if>
        <xsl:if test="MST = ''">
          <MST />
        </xsl:if>
      </xsl:if>

      <xsl:if test="DChi">
        <xsl:if test="DChi != ''">
          <DChi>
            <xsl:value-of select="DChi" />
          </DChi>
        </xsl:if>
        <xsl:if test="DChi = ''">
          <DChi />
        </xsl:if>
      </xsl:if>

      <xsl:if test="SDThoai">
        <xsl:if test="SDThoai != ''">
          <SDThoai>
            <xsl:value-of select="SDThoai" />
          </SDThoai>
        </xsl:if>
        <xsl:if test="SDThoai = ''">
          <SDThoai />
        </xsl:if>
      </xsl:if>

      <xsl:if test="DCTDTu">
        <xsl:if test="DCTDTu != ''">
          <DCTDTu>
            <xsl:value-of select="DCTDTu" />
          </DCTDTu>
        </xsl:if>
        <xsl:if test="DCTDTu = ''">
          <DCTDTu />
        </xsl:if>
      </xsl:if>

      <xsl:if test="STKNHang">
        <xsl:if test="STKNHang != ''">
          <STKNHang>
            <xsl:value-of select="STKNHang" />
          </STKNHang>
        </xsl:if>
        <xsl:if test="STKNHang = ''">
          <STKNHang />
        </xsl:if>
      </xsl:if>

      <xsl:if test="TNHang">
        <xsl:if test="TNHang != ''">
          <TNHang>
            <xsl:value-of select="TNHang" />
          </TNHang>
        </xsl:if>
        <xsl:if test="TNHang = ''">
          <TNHang />
        </xsl:if>
      </xsl:if>

      <xsl:if test="Fax">
        <xsl:if test="Fax != ''">
          <Fax>
            <xsl:value-of select="Fax" />
          </Fax>
        </xsl:if>
        <xsl:if test="Fax = ''">
          <Fax />
        </xsl:if>
      </xsl:if>

      <xsl:if test="LDDNBo">
        <xsl:if test="LDDNBo != ''">
          <LDDNBo>
            <xsl:value-of select="LDDNBo" />
          </LDDNBo>
        </xsl:if>
        <xsl:if test="LDDNBo = ''">
          <LDDNBo />
        </xsl:if>
      </xsl:if>

      <xsl:if test="HDSo">
        <xsl:if test="HDSo != ''">
          <HDSo>
            <xsl:value-of select="HDSo" />
          </HDSo>
        </xsl:if>
        <xsl:if test="HDSo = ''">
          <HDSo />
        </xsl:if>
      </xsl:if>

      <xsl:if test="HVTNXHang">
        <xsl:if test="HVTNXHang != ''">
          <HVTNXHang>
            <xsl:value-of select="HVTNXHang" />
          </HVTNXHang>
        </xsl:if>
        <xsl:if test="HVTNXHang = ''">
          <HVTNXHang />
        </xsl:if>
      </xsl:if>

      <xsl:if test="TNVChuyen">
        <xsl:if test="TNVChuyen != ''">
          <TNVChuyen>
            <xsl:value-of select="TNVChuyen" />
          </TNVChuyen>
        </xsl:if>
        <xsl:if test="TNVChuyen = ''">
          <TNVChuyen />
        </xsl:if>
      </xsl:if>

      <xsl:if test="PTVChuyen">
        <xsl:if test="PTVChuyen != ''">
          <PTVChuyen>
            <xsl:value-of select="PTVChuyen" />
          </PTVChuyen>
        </xsl:if>
        <xsl:if test="PTVChuyen = ''">
          <PTVChuyen />
        </xsl:if>
      </xsl:if>

    </NBan>
  </xsl:template>

  <xsl:template match="NMua">
    <NMua>

      <xsl:if test="Ten">
        <xsl:if test="Ten != ''">
          <Ten>
            <xsl:value-of select="Ten" />
          </Ten>
        </xsl:if>
        <xsl:if test="Ten = ''">
          <Ten />
        </xsl:if>
      </xsl:if>

      <xsl:if test="MST">
        <xsl:if test="MST != ''">
          <MST>
            <xsl:value-of select="MST" />
          </MST>
        </xsl:if>
        <xsl:if test="MST = ''">
          <MST />
        </xsl:if>
      </xsl:if>

      <xsl:if test="DChi">
        <xsl:if test="DChi != ''">
          <DChi>
            <xsl:value-of select="DChi" />
          </DChi>
        </xsl:if>
        <xsl:if test="DChi = ''">
          <DChi />
        </xsl:if>
      </xsl:if>

      <xsl:if test="MKHang">
        <xsl:if test="MKHang != ''">
          <MKHang>
            <xsl:value-of select="MKHang" />
          </MKHang>
        </xsl:if>
        <xsl:if test="MKHang = ''">
          <MKHang />
        </xsl:if>
      </xsl:if>

    </NMua>
  </xsl:template>

  <xsl:template match="DSHHDVu">
    <DSHHDVu>
      <xsl:for-each select="HHDVu">
        <HHDVu>

          <xsl:if test="TChat">
            <xsl:if test="TChat != ''">
              <TChat>
                <xsl:value-of select="TChat" />
              </TChat>
            </xsl:if>
            <xsl:if test="TChat = ''">
              <TChat />
            </xsl:if>
          </xsl:if>

          <xsl:if test="STT">
            <xsl:if test="STT != ''">
              <STT>
                <xsl:value-of select="STT" />
              </STT>
            </xsl:if>
            <xsl:if test="STT = ''">
              <STT />
            </xsl:if>
          </xsl:if>

          <xsl:if test="MHHDVu">
            <xsl:if test="MHHDVu != ''">
              <MHHDVu>
                <xsl:value-of select="MHHDVu" />
              </MHHDVu>
            </xsl:if>
            <xsl:if test="MHHDVu = ''">
              <MHHDVu />
            </xsl:if>
          </xsl:if>

          <xsl:if test="THHDVu">
            <xsl:if test="THHDVu != ''">
              <THHDVu>
                <xsl:value-of select="THHDVu" />
              </THHDVu>
            </xsl:if>
            <xsl:if test="THHDVu = ''">
              <THHDVu />
            </xsl:if>
          </xsl:if>

          <xsl:if test="DVTinh">
            <xsl:if test="DVTinh != ''">
              <DVTinh>
                <xsl:value-of select="DVTinh" />
              </DVTinh>
            </xsl:if>
            <xsl:if test="DVTinh = ''">
              <DVTinh />
            </xsl:if>
          </xsl:if>

          <!--SLuong-->
          <xsl:if test="SLuong">
            <xsl:if test="SLuong != ''">
              <SLuong>
                <xsl:value-of select="SLuong" />
              </SLuong>
            </xsl:if>
            <xsl:if test="SLuong = ''">
              <SLuong />
            </xsl:if>
          </xsl:if>

          <!--DGia-->
          <xsl:if test="DGia">
            <xsl:if test="DGia != ''">
              <DGia>
                <xsl:value-of select="DGia" />
              </DGia>
            </xsl:if>
            <xsl:if test="DGia = ''">
              <DGia />
            </xsl:if>
          </xsl:if>

          <!--ThTien-->
          <xsl:if test="ThTien">
            <xsl:if test="ThTien != ''">
              <ThTien>
                <xsl:value-of select="ThTien" />
              </ThTien>
            </xsl:if>
            <xsl:if test="ThTien = ''">
              <ThTien />
            </xsl:if>
          </xsl:if>

          <!--TSuat-->
          <xsl:if test="TSuat">
            <xsl:if test="TSuat != ''">
              <TSuat>
                <xsl:value-of select="TSuat" />
              </TSuat>
            </xsl:if>
            <xsl:if test="TSuat = ''">
              <TSuat />
            </xsl:if>
          </xsl:if>

          <!--TThue-->
          <xsl:if test="TThue">
            <xsl:if test="TThue != ''">
              <TThue>
                <xsl:value-of select="TThue" />
              </TThue>
            </xsl:if>
            <xsl:if test="TThue = ''">
              <TThue />
            </xsl:if>
          </xsl:if>

          <!--TSThue-->
          <xsl:if test="TSThue">
            <xsl:if test="TSThue != ''">
              <TSThue>
                <xsl:value-of select="TSThue" />
              </TSThue>
            </xsl:if>
            <xsl:if test="TSThue = ''">
              <TSThue />
            </xsl:if>
          </xsl:if>
        </HHDVu>
      </xsl:for-each>
    </DSHHDVu>
  </xsl:template>

  <xsl:template match="TToan">
    <TToan>
      <xsl:apply-templates select="THTTLTSuat" />

      <xsl:if test="TgTCThue">
        <xsl:if test="TgTCThue != ''">
          <TgTCThue>
            <xsl:value-of select="TgTCThue" />
          </TgTCThue>
        </xsl:if>
        <xsl:if test="TgTCThue = ''">
          <TgTCThue />
        </xsl:if>
      </xsl:if>

      <xsl:if test="TgTThue">
        <xsl:if test="TgTThue != ''">
          <TgTThue>
            <xsl:value-of select="TgTThue" />
          </TgTThue>
        </xsl:if>
        <xsl:if test="TgTThue = ''">
          <TgTThue />
        </xsl:if>
      </xsl:if>

      <xsl:if test="TgTTTBSo">
        <xsl:if test="TgTTTBSo != ''">
          <TgTTTBSo>
            <xsl:value-of select="TgTTTBSo" />
          </TgTTTBSo>
        </xsl:if>
        <xsl:if test="TgTTTBSo = ''">
          <TgTTTBSo />
        </xsl:if>
      </xsl:if>

      <xsl:if test="TgTTTBChu">
        <xsl:if test="TgTTTBChu != ''">
          <TgTTTBChu>
            <xsl:value-of select="TgTTTBChu" />
          </TgTTTBChu>
        </xsl:if>
        <xsl:if test="TgTTTBChu = ''">
          <TgTTTBChu />
        </xsl:if>
      </xsl:if>

    </TToan>
  </xsl:template>

  <xsl:template match="THTTLTSuat">
    <THTTLTSuat>
      <xsl:apply-templates select="LTSuat" />

    </THTTLTSuat>
  </xsl:template>

  <xsl:template match="LTSuat">
    <LTSuat>

      <xsl:if test="TSuat">
        <xsl:if test="TSuat != ''">
          <TSuat>
            <xsl:value-of select="TSuat" />
          </TSuat>
        </xsl:if>
        <xsl:if test="TSuat = ''">
          <TSuat />
        </xsl:if>
      </xsl:if>

      <xsl:if test="ThTien">
        <xsl:if test="ThTien != ''">
          <ThTien>
            <xsl:value-of select="ThTien" />
          </ThTien>
        </xsl:if>
        <xsl:if test="ThTien = ''">
          <ThTien />
        </xsl:if>
      </xsl:if>

      <xsl:if test="TThue">
        <xsl:if test="TThue != ''">
          <TThue>
            <xsl:value-of select="TThue" />
          </TThue>
        </xsl:if>
        <xsl:if test="TThue = ''">
          <TThue />
        </xsl:if>
      </xsl:if>

    </LTSuat>
  </xsl:template>
</xsl:stylesheet>
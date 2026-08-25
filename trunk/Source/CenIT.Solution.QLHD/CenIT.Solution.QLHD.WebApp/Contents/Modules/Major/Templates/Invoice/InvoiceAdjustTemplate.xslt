<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />

  <xsl:template match="/AdjustInv">
    <AdjustInv>
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

      <xsl:if test="CusCode">
        <xsl:if test="CusCode != ''">
          <CusCode>
            <xsl:value-of select="CusCode" />
          </CusCode>
        </xsl:if>
        <xsl:if test="CusCode = ''">
          <CusCode />
        </xsl:if>
      </xsl:if>

      <xsl:if test="CusBankNo">
        <xsl:if test="CusBankNo != ''">
          <CusBankNo>
            <xsl:value-of select="CusBankNo" />
          </CusBankNo>
        </xsl:if>
        <xsl:if test="CusBankNo = ''">
          <CusBankNo />
        </xsl:if>
      </xsl:if>

      <xsl:if test="Buyer">
        <xsl:if test="Buyer != ''">
          <Buyer>
            <xsl:value-of select="Buyer" />
          </Buyer>
        </xsl:if>
        <xsl:if test="Buyer = ''">
          <Buyer />
        </xsl:if>
      </xsl:if>

      <xsl:if test="CusName">
        <xsl:if test="CusName != ''">
          <CusName>
            <xsl:value-of select="CusName" />
          </CusName>
        </xsl:if>
        <xsl:if test="CusName = ''">
          <CusName />
        </xsl:if>
      </xsl:if>

      <xsl:if test="CusAddress">
        <xsl:if test="CusAddress != ''">
          <CusAddress>
            <xsl:value-of select="CusAddress" />
          </CusAddress>
        </xsl:if>
        <xsl:if test="CusAddress = ''">
          <CusAddress />
        </xsl:if>
      </xsl:if>

      <xsl:if test="CusPhone">
        <xsl:if test="CusPhone != ''">
          <CusPhone>
            <xsl:value-of select="CusPhone" />
          </CusPhone>
        </xsl:if>
        <xsl:if test="CusPhone = ''">
          <CusPhone />
        </xsl:if>
      </xsl:if>

      <xsl:if test="CusTaxCode">
        <xsl:if test="CusTaxCode != ''">
          <CusTaxCode>
            <xsl:value-of select="CusTaxCode" />
          </CusTaxCode>
        </xsl:if>
        <xsl:if test="CusTaxCode = ''">
          <CusTaxCode />
        </xsl:if>
      </xsl:if>

      <xsl:if test="PaymentMethod">
        <xsl:if test="PaymentMethod != ''">
          <PaymentMethod>
            <xsl:value-of select="PaymentMethod" />
          </PaymentMethod>
        </xsl:if>
        <xsl:if test="PaymentMethod = ''">
          <PaymentMethod />
        </xsl:if>
      </xsl:if>

      <xsl:if test="KindOfService">
        <xsl:if test="KindOfService != ''">
          <KindOfService>
            <xsl:value-of select="KindOfService" />
          </KindOfService>
        </xsl:if>
        <xsl:if test="KindOfService = ''">
          <KindOfService />
        </xsl:if>
      </xsl:if>

      <xsl:if test="Type">
        <xsl:if test="Type != ''">
          <Type>
            <xsl:value-of select="Type" />
          </Type>
        </xsl:if>
        <xsl:if test="Type = ''">
          <Type />
        </xsl:if>
      </xsl:if>

      <xsl:apply-templates select="Products" />

      <xsl:if test="Total">
        <xsl:if test="Total != ''">
          <Total>
            <xsl:value-of select="Total" />
          </Total>
        </xsl:if>
        <xsl:if test="Total = ''">
          <Total />
        </xsl:if>
      </xsl:if>

      <xsl:if test="DiscountAmount">
        <xsl:if test="DiscountAmount != ''">
          <DiscountAmount>
            <xsl:value-of select="DiscountAmount" />
          </DiscountAmount>
        </xsl:if>
        <xsl:if test="DiscountAmount = ''">
          <DiscountAmount />
        </xsl:if>
      </xsl:if>

      <xsl:if test="VATRate">
        <xsl:if test="VATRate != ''">
          <VATRate>
            <xsl:value-of select="VATRate" />
          </VATRate>
        </xsl:if>
        <xsl:if test="VATRate = ''">
          <VATRate />
        </xsl:if>
      </xsl:if>

      <xsl:if test="VATAmount">
        <xsl:if test="VATAmount != ''">
          <VATAmount>
            <xsl:value-of select="VATAmount" />
          </VATAmount>
        </xsl:if>
        <xsl:if test="VATAmount = ''">
          <VATAmount />
        </xsl:if>
      </xsl:if>

      <xsl:if test="Amount">
        <xsl:if test="Amount != ''">
          <Amount>
            <xsl:value-of select="Amount" />
          </Amount>
        </xsl:if>
        <xsl:if test="Amount = ''">
          <Amount />
        </xsl:if>
      </xsl:if>

      <xsl:if test="AmountInWords">
        <xsl:if test="AmountInWords != ''">
          <AmountInWords>
            <xsl:value-of select="AmountInWords" />
          </AmountInWords>
        </xsl:if>
        <xsl:if test="AmountInWords = ''">
          <AmountInWords />
        </xsl:if>
      </xsl:if>

      <xsl:if test="Extra">
        <xsl:if test="Extra != ''">
          <Extra>
            <xsl:value-of select="Extra" />
          </Extra>
        </xsl:if>
        <xsl:if test="Extra = ''">
          <Extra />
        </xsl:if>
      </xsl:if>

      <!--GrossValue-->
      <xsl:if test="GrossValue">
        <xsl:if test="GrossValue != ''">
          <GrossValue>
            <xsl:value-of select="GrossValue" />
          </GrossValue>
        </xsl:if>
        <xsl:if test="GrossValue = ''">
          <GrossValue />
        </xsl:if>
      </xsl:if>

      <!--GrossValue0-->
      <xsl:if test="GrossValue0">
        <xsl:if test="GrossValue0 != ''">
          <GrossValue0>
            <xsl:value-of select="GrossValue0" />
          </GrossValue0>
        </xsl:if>
        <xsl:if test="GrossValue0 = ''">
          <GrossValue0 />
        </xsl:if>
      </xsl:if>

      <!--VatAmount0-->
      <xsl:if test="VatAmount0">
        <xsl:if test="VatAmount0 != ''">
          <VatAmount0>
            <xsl:value-of select="VatAmount0" />
          </VatAmount0>
        </xsl:if>
        <xsl:if test="VatAmount0 = ''">
          <VatAmount0 />
        </xsl:if>
      </xsl:if>

      <!--GrossValue5-->
      <xsl:if test="GrossValue5">
        <xsl:if test="GrossValue5 != ''">
          <GrossValue5>
            <xsl:value-of select="GrossValue5" />
          </GrossValue5>
        </xsl:if>
        <xsl:if test="GrossValue5 = ''">
          <GrossValue5 />
        </xsl:if>
      </xsl:if>

      <!--VatAmount5-->
      <xsl:if test="VatAmount5">
        <xsl:if test="VatAmount5 != ''">
          <VatAmount5>
            <xsl:value-of select="VatAmount5" />
          </VatAmount5>
        </xsl:if>
        <xsl:if test="VatAmount5 = ''">
          <VatAmount5 />
        </xsl:if>
      </xsl:if>

      <!--GrossValue10-->
      <xsl:if test="GrossValue10">
        <xsl:if test="GrossValue10 != ''">
          <GrossValue10>
            <xsl:value-of select="GrossValue10" />
          </GrossValue10>
        </xsl:if>
        <xsl:if test="GrossValue10 = ''">
          <GrossValue10 />
        </xsl:if>
      </xsl:if>

      <!--VatAmount10-->
      <xsl:if test="VatAmount10">
        <xsl:if test="VatAmount10 != ''">
          <VatAmount10>
            <xsl:value-of select="VatAmount10" />
          </VatAmount10>
        </xsl:if>
        <xsl:if test="VatAmount10 = ''">
          <VatAmount10 />
        </xsl:if>
      </xsl:if>

    </AdjustInv>
  </xsl:template>

  <xsl:template match="Products">
    <Products>
      <xsl:for-each select="Product">
        <Product>

          <xsl:if test="ProdName">
            <xsl:if test="ProdName != ''">
              <ProdName>
                <xsl:value-of select="ProdName" />
              </ProdName>
            </xsl:if>
            <xsl:if test="ProdName = ''">
              <ProdName />
            </xsl:if>
          </xsl:if>

          <xsl:if test="ProdUnit">
            <xsl:if test="ProdUnit != ''">
              <ProdUnit>
                <xsl:value-of select="ProdUnit" />
              </ProdUnit>
            </xsl:if>
            <xsl:if test="ProdUnit = ''">
              <ProdUnit />
            </xsl:if>
          </xsl:if>

          <xsl:if test="ProdQuantity">
            <xsl:if test="ProdQuantity != ''">
              <ProdQuantity>
                <xsl:value-of select="ProdQuantity" />
              </ProdQuantity>
            </xsl:if>
            <xsl:if test="ProdQuantity = ''">
              <ProdQuantity />
            </xsl:if>
          </xsl:if>

          <xsl:if test="ProdPrice">
            <xsl:if test="ProdPrice != ''">
              <ProdPrice>
                <xsl:value-of select="ProdPrice" />
              </ProdPrice>
            </xsl:if>
            <xsl:if test="ProdPrice = ''">
              <ProdPrice />
            </xsl:if>
          </xsl:if>

          <xsl:if test="Amount">
            <xsl:if test="Amount != ''">
              <Amount>
                <xsl:value-of select="Amount" />
              </Amount>
            </xsl:if>
            <xsl:if test="Amount = ''">
              <Amount />
            </xsl:if>
          </xsl:if>

          <!--Total-->
          <xsl:if test="Total">
            <xsl:if test="Total != ''">
              <Total>
                <xsl:value-of select="Total" />
              </Total>
            </xsl:if>
            <xsl:if test="Total = ''">
              <Total />
            </xsl:if>
          </xsl:if>
          <!--IsSum-->
          <xsl:if test="IsSum">
            <xsl:if test="IsSum != ''">
              <IsSum>
                <xsl:value-of select="IsSum" />
              </IsSum>
            </xsl:if>
            <xsl:if test="IsSum = ''">
              <IsSum>0</IsSum>
            </xsl:if>
          </xsl:if>
        </Product>
      </xsl:for-each>
    </Products>
  </xsl:template>

</xsl:stylesheet>
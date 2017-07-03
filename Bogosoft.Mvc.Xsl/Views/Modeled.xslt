<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- Model Templates -->
	<xsl:template match="/*">
		<xsl:text>Override me!</xsl:text>
	</xsl:template>
	<!-- Structural Templates -->
	<xsl:template match="/" mode="html.body.content">
		<xsl:apply-templates select="child::*[1]" />
	</xsl:template>
</xsl:stylesheet>
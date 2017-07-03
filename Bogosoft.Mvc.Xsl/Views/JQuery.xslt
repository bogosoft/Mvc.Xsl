<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- Imports -->
	<xsl:import href="Base.xslt" />
	<!-- Parameters -->
	<xsl:param name="jquery-path" select="''" />
	<!-- Structural Templates -->
	<xsl:template match="/" mode="html.body.scripts">
		<xsl:if test="string-length($jquery-path) &gt; 0">
			<script src="{$jquery-path}" />
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
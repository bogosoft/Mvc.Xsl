<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- Imports -->
	<xsl:import href="JQuery.xslt" />
	<!-- Parameters -->
	<xsl:param name="bootstrap-css-path" select="'Content/bootstrap.min.css'" />
	<xsl:param name="bootstrap-js-path" select="'Scripts/bootstrap.min.js'" />
	<!-- Structural Templates -->
	<xsl:template match="/" mode="html.head.links">
		<xsl:apply-imports />
		<link href="{$bootstrap-css-path}" rel="stylesheet" />
	</xsl:template>
	<xsl:template match="/" mode="html.body.scripts">
		<xsl:apply-imports />
		<script src="{$bootstrap-js-path}" />
	</xsl:template>
</xsl:stylesheet>
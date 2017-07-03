﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- Imports -->
	<xsl:import href="../Bootstrap.xslt" />
	<!-- Parameters -->
	<xsl:param name="action" select="''" />
	<xsl:param name="jquery-path" select="'Scripts/jquery-3.1.1.min.js'" />
	<xsl:param name="controller" select="''" />
	<!-- Structural templates -->
	<xsl:template match="/" mode="html.head.links">
		<xsl:apply-imports />
		<link href="content/Site.css" rel="stylesheet" />
	</xsl:template>
	<xsl:template match="/" mode="html.body.content">
		<p>
			<xsl:text>Hello, </xsl:text>
			<xsl:value-of select="$controller" />
			<xsl:text>.</xsl:text>
			<xsl:value-of select="$action" />
			<xsl:text>!</xsl:text>
		</p>
	</xsl:template>
</xsl:stylesheet>
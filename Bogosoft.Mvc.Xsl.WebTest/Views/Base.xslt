<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- Output settings -->
	<xsl:output method="xml" indent="yes"/>
	<!-- Global parameters -->
	<xsl:param name="charset" select="'utf-8'" />
	<xsl:param name="page-title" select="'Example Page'" />
	<!-- Model templates -->
	<xsl:template match="/">
		<xsl:apply-templates select="." mode="html" />
	</xsl:template>
	<xsl:template match="/*">
		<p>Override me!</p>
	</xsl:template>
	<!-- Structural templates -->
	<xsl:template match="/" mode="html">
		<html>
			<xsl:apply-templates select="." mode="html.attributes" />
			<xsl:apply-templates select="." mode="html.head" />
			<xsl:apply-templates select="." mode="html.body" />
		</html>
	</xsl:template>
	<xsl:template match="/" mode="html.attributes" />
	<xsl:template match="/" mode="html.head">
		<head>
			<xsl:if test="string-length($charset) &gt; 0">
				<meta charset="{$charset}" />
			</xsl:if>
			<xsl:apply-templates select="." mode="html.head.meta" />
			<xsl:apply-templates select="." mode="html.head.title" />
			<xsl:apply-templates select="." mode="html.head.links" />
		</head>
	</xsl:template>
	<xsl:template match="/" mode="html.body">
		<body>
			<xsl:apply-templates select="." mode="html.body.attributes" />
			<xsl:apply-templates select="." mode="html.body.content" />
			<xsl:apply-templates select="." mode="html.body.scripts" />
		</body>
	</xsl:template>
	<xsl:template match="/" mode="html.head.meta" />
	<xsl:template match="/" mode="html.head.title">
		<title>
			<xsl:apply-templates select="." mode="html.head.title.text" />
		</title>
	</xsl:template>
	<xsl:template match="/" mode="html.head.links" />
	<xsl:template match="/" mode="html.body.attributes" />
	<xsl:template match="/" mode="html.body.content">
		<xsl:apply-templates select="." mode="html.body.content.no-model" />
		<xsl:apply-templates select="child::*[1]" />
	</xsl:template>
	<xsl:template match="/" mode="html.body.scripts" />
	<xsl:template match="/" mode="html.head.title.text">
		<xsl:value-of select="$page-title" />
	</xsl:template>
	<!-- This template should be overridden for any view which does not require an actual model. -->
	<xsl:template match="/" mode="html.body.content.no-model" />
</xsl:stylesheet>
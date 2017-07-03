<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- Output settings -->
	<xsl:output method="xml" indent="no"/>
	<!-- Global parameters -->
	<xsl:param name="page-title" select="'Example Page'" />
	<!-- Model templates -->
	<xsl:template match="/">
		<xsl:apply-templates select="." mode="html" />
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
			<xsl:apply-templates select="." mode="html.head.attributes" />
			<xsl:apply-templates select="." mode="html.head.meta" />
			<xsl:apply-templates select="." mode="html.head.title" />
			<xsl:apply-templates select="." mode="html.head.links" />
			<xsl:apply-templates select="." mode="html.head.scripts" />
		</head>
	</xsl:template>
	<xsl:template match="/" mode="html.body">
		<body>
			<xsl:apply-templates select="." mode="html.body.attributes" />
			<xsl:apply-templates select="." mode="html.body.container" />
			<xsl:apply-templates select="." mode="html.body.scripts" />
		</body>
	</xsl:template>
	<xsl:template match="/" mode="html.head.attributes" />
	<xsl:template match="/" mode="html.head.meta">
		<meta charset="utf-8" />
	</xsl:template>
	<xsl:template match="/" mode="html.head.title">
		<title>
			<xsl:apply-templates select="." mode="html.head.title.text" />
		</title>
	</xsl:template>
	<xsl:template match="/" mode="html.head.links">
		<link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
	</xsl:template>
	<xsl:template match="/" mode="html.head.scripts" />
	<xsl:template match="/" mode="html.body.attributes" />
	<xsl:template match="/" mode="html.body.container">
		<xsl:apply-templates select="." mode="html.body.header" />
		<xsl:apply-templates select="." mode="html.body.content" />
		<xsl:apply-templates select="." mode="html.body.footer" />
	</xsl:template>
	<xsl:template match="/" mode="html.body.scripts" />
	<xsl:template match="/" mode="html.head.title.text">
		<xsl:value-of select="$page-title" />
	</xsl:template>
	<xsl:template match="/" mode="html.body.content">
		<xsl:apply-templates select="." mode="html.body.header" />
		<xsl:apply-templates select="." mode="html.body.content" />
		<xsl:apply-templates select="." mode="html.body.footer" />
	</xsl:template>
</xsl:stylesheet>
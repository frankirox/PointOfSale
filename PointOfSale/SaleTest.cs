﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace PointOfSale.Domain
{
	public class SaleTest
	{
		int numOfDecimalPlaces = 1;

		[Theory]
		[InlineData("123456", 12.50)]
		[InlineData("456789", 24.50)]
		[InlineData("900000", 7.50)]
		public void GetTotalPriceForOneItemReturnsCorrectResult(string barcode, decimal expectedPrice)
		{
			// Arrange
			var itemRepo = new ItemRegistry();
			var dummy = new Mock<Display>();
			var sut = new Sale(itemRepo, dummy.Object);

			// Act
			sut.Scan(barcode);

			// Assert
			Assert.Equal(expectedPrice, sut.TotalPrice, numOfDecimalPlaces);
		}


		[Theory]
		[InlineData(new string[2] { "123456", "456789" }, 37.00)]
		public void GetTotalPriceForTwoItemsReturnsTotalPrice(string[] barcodes, decimal expectedTotalPrice)
		{
			// Arrange
			var itemRepo = new ItemRegistry();
			var dummy = new Mock<Display>();
			var sut = new Sale(itemRepo, dummy.Object);

			// Act
			barcodes.ToList().ForEach(barcode => sut.Scan(barcode));

			// Assert
			Assert.Equal(expectedTotalPrice, sut.TotalPrice, numOfDecimalPlaces);
		}


		[Fact]
		public void GetTotalPriceOnEmptyBarcodeReturns0Price()
		{

			// Arrange
			var itemRepo = new ItemRegistry();
			var dummy = new Mock<Display>();
			var sut = new Sale(itemRepo, dummy.Object);
			// Act
			sut.Scan("");
			// Assert
			var expected = new decimal(0);
			Assert.Equal(expected, sut.TotalPrice, numOfDecimalPlaces);
		}

		[Theory]
		[InlineData("123456")]
		public void ItemWithRegisteredBarcodeIsStoredInsideScannedItems(string barcode)
		{
			// Arrange
			var itemRepo = new ItemRegistry();
			var dummy = new Mock<Display>();
			var sut = new Sale(itemRepo, dummy.Object);
			var expected = itemRepo.getItemWith(barcode);
			// Act
			sut.Scan(barcode);
			// Assert
			sut.ScannedItems.Should().Contain(expected);
		}


		[Fact]
		public void ScannedItemIsDisplayed()
		{
			// Arrange
			var itemRepo = new ItemRegistry();
			var sut = new Mock<Display>();
			var sale = new Sale(itemRepo, sut.Object);
			var item = itemRepo.getItemWith("123456");
			// Act
			sale.Scan("123456");

			// Assert
			sut.Verify(s => s.DisplayScannedItem(item));
		}
	}

}

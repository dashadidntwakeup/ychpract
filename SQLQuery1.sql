SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartnerSales](
    [Id_sale] [int] IDENTITY(1,1) NOT NULL,
    [PartnerId] [int] NOT NULL,
    [SaleDate] [datetime] NOT NULL,
    [Amount] [decimal](18,2) NOT NULL,
    [ProductName] [nvarchar](200) NULL,
    [Quantity] [int] NOT NULL,
 CONSTRAINT [PK_PartnerSales] PRIMARY KEY CLUSTERED 
(
    [Id_sale] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]  -- убрано TEXTIMAGE_ON
GO

ALTER TABLE [dbo].[PartnerSales]  WITH CHECK ADD  CONSTRAINT [FK_PartnerSales_Partners] FOREIGN KEY([PartnerId])
REFERENCES [dbo].[Partners] ([Id_partner])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PartnerSales] CHECK CONSTRAINT [FK_PartnerSales_Partners]
GO

-- Создаём индексы для быстрой выборки
CREATE NONCLUSTERED INDEX IX_PartnerSales_PartnerId
    ON [dbo].[PartnerSales] (PartnerId);
GO

CREATE NONCLUSTERED INDEX IX_PartnerSales_SaleDate
    ON [dbo].[PartnerSales] (SaleDate DESC);
GO


INSERT INTO [dbo].[PartnerSales] (PartnerId, SaleDate, Amount, ProductName, Quantity)
VALUES 
(1, '2026-02-15T10:30:00', 321014.00, N'Комплект мебели для гостиной Ольха горная', 2),
(1, '2026-02-20T14:15:00', 74910.00, N'Прихожая Венге Винтаж', 3),
(1, '2026-03-05T09:45:00', 36412.00, N'Тумба с вешалкой Дуб натуральный', 2),
(2, '2026-02-25T11:00:00', 216907.00, N'Стенка для гостиной Вишня темная', 1),
(2, '2026-03-10T16:20:00', 177509.00, N'Прихожая-комплект Дуб темный', 1),
(3, '2026-03-01T13:30:00', 85900.00, N'Диван-кровать угловой Книжка', 1),
(3, '2026-03-12T10:15:00', 75900.00, N'Диван модульный Телескоп', 1),
(4, '2026-03-18T12:00:00', 120345.00, N'Диван-кровать Соло', 1),
(5, '2026-03-20T15:45:00', 25990.00, N'Детский диван Выкатной', 1),
(5, '2026-03-22T09:30:00', 69500.00, N'Кровать с подъемным механизмом с матрасом 1600х2000 Венге', 1),
(6, '2026-03-25T14:00:00', 131560.00, N'Шкаф-купе 3-х дверный Сосна белая', 1),
(7, '2026-03-28T11:45:00', 46750.00, N'Кровать с ящиками Ясень белый', 1),
(8, '2026-04-01T10:00:00', 160151.00, N'Шкаф 4 дверный с ящиками Ясень серый', 1),
(9, '2026-04-03T16:30:00', 61235.00, N'Комод 6 ящиков Вишня светлая', 1),
(10, '2026-04-05T12:15:00', 82400.00, N'Комод 4 ящика Вишня светлая', 2);
SELECT * FROM PartnerSales;
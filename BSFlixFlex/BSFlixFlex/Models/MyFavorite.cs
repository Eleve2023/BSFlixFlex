using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSFlixFlex.Client.Shareds.Models;

namespace BSFlixFlex.Models
{
    public class MyFavorite
    {
        [Key] public int Id { get; set; }
        public int IdCinematography {  get; set; }
        public Guid UserId { get; set; }

        [DataType(DataType.Text),Column(TypeName = "text")]
        public Cinematography Cinematography { get; set; }
    }
    public class PersonConfigue : IEntityTypeConfiguration<MyFavorite>
    {
        public void Configure(EntityTypeBuilder<MyFavorite> builder)
        {            
                 
        }

    }
}
